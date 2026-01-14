using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using LicenseService.Data;
using LicenseService.Entity;
using LicenseService.Helper;
using LicenseService.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace LicenseService.Service.Impl;

public class LicensingService(IOptions<AppConfigSetting> options, AppDbContext context) : ILicenseService
{
  private readonly ECDsa _signer = ECDsa.Create(ECCurve.NamedCurves.nistP256);
  private readonly AppConfigSetting _settings = options.Value;

  public Task<EncryptedLicense> CreateLicenseAsync(LicensePayload payload)
  {
    throw new NotImplementedException();
  }

  public async Task<string> TrustServerAsync(TrustServerDto dto)
  {
    // Step 1 : Generate ECDH key pair
    // var (publicKey, privateKey) = EcdhCryptoHelper.GenerateEcdhKeyPair();

    if (string.IsNullOrEmpty(dto.peerPublicKey) || string.IsNullOrEmpty(dto.machineId))
      return null;

    // Step 1 : Get keys pair from DB
    var key = await context.key_pair
    .AsNoTracking()
    .FirstOrDefaultAsync(k => k.is_revoked == false);

    if (key == null) return null;

    // Step 2 : Derive shared secret
    var serverPublicKeyBytes = Convert.FromBase64String(dto.peerPublicKey);
    var sharedSecret = EcdhCryptoHelper.DeriveSharedSecret(key.secret_key, serverPublicKeyBytes);

    // Step 3 : Store the public key and shared secret in the database
    var secret = new SignKeyAudit
    {
      key_uuid = key.key_uuid,
      machine_id = dto.machineId,
      public_key = key.public_key,
      shared_secret = sharedSecret,
      created_date = DateTime.UtcNow,
      expire_date = DateTime.UtcNow.AddDays(_settings.Encryption.KeyExpireInMonth),
      is_revoked = false,
    };
    await context.secret.AddAsync(secret);
    await context.SaveChangesAsync();

    return Convert.ToBase64String(key.public_key);
  }

  public async Task<EncryptedLicense> CreateLicenseDemoAsync(GenerateDemo demo)
  {
    var secrets = await context.secret.AsNoTracking().FirstOrDefaultAsync(x => x.is_revoked == false && x.machine_id == demo.machineId);

    if (secrets == null) return null;

    var isAvailable = await context.license.AsNoTracking().AnyAsync(x => x.machine_id.Equals(demo.machineId));

    if (isAvailable) return null;

    var payload = new LicensePayload(
      Guid.NewGuid(),
      demo.company,
      demo.machineId,
      _settings.DemoLicense.nHardware,
      _settings.DemoLicense.nModule,
      _settings.DemoLicense.nOperator,
      _settings.DemoLicense.nLocation,
      _settings.DemoLicense.nControl,
      _settings.DemoLicense.nMonitor,
      _settings.DemoLicense.nMonitorGroup,
      _settings.DemoLicense.nDoor,
      _settings.DemoLicense.nAccessLevle,
      _settings.DemoLicense.nTimezone,
      _settings.DemoLicense.nCard,
      _settings.DemoLicense.nCardHolder,
      _settings.DemoLicense.nTrigger,
      _settings.DemoLicense.nHoliday,
      DateTime.Now,
      DateTime.Now.AddDays(_settings.DemoLicense.DurationInDays), // Demo license valid days based on nHardware count
      _settings.DemoLicense.GracePeriodInDays,
      false
    );

    var json = JsonSerializer.Serialize(payload);
    var data = Encoding.UTF8.GetBytes(json);

    // Encrypt license with shared secret
    var encrypt = SymmetricEncryptHelper.Encrypt(data, secrets.shared_secret);
    // Sign license with encypt data with private key
    var sign = _signer.SignData(encrypt, HashAlgorithmName.SHA256);

    var license = new EncryptedLicense(
      secrets.secret_uuid,
      Convert.ToBase64String(encrypt),
      Convert.ToBase64String(sign)
    );

    var en = new Entity.License
    {
      company = demo.company,
      customer_site = demo.customerSite,
      machine_id = demo.machineId,
      secret_uuid = secrets.secret_uuid,
      license = encrypt,
      license_type = Enums.LicenseType.Demo,
      created_date = DateTime.Now,
      expire_date = DateTime.Now.AddDays(_settings.DemoLicense.DurationInDays),
    };

    File.WriteAllText("license.json", JsonSerializer.Serialize(license, new JsonSerializerOptions { WriteIndented = true }));
    Console.WriteLine("License generated.");

    await context.license.AddAsync(en);
    await context.SaveChangesAsync();


    return license;
  }
}
