using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using LicenseService.Data;
using LicenseService.Helper;
using LicenseService.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace LicenseService.Service.Impl;

public class LicenseService : ILicenseService
{
  private readonly AppConfigSetting _settings;
  private readonly AppDbContext _contex;
  public LicenseService(IOptions<AppConfigSetting> options, AppDbContext contex)
  {
    _settings = options.Value;
    _contex = contex;
  }
  public Task<EncryptedLicense> CreateLicenseAsync(LicensePayload payload)
  {
    throw new NotImplementedException();
  }

  public async Task<EncryptedLicense> CreateLicenseDemoAsync(string fingerPrint)
  {
    var keys = await _contex.Keys.AsNoTracking().FirstOrDefaultAsync();

    if (keys == null) return null;

    //var privateKeyPem = File.ReadAllText("license_private.pem");
    var aesKey = SHA256.HashData(Encoding.UTF8.GetBytes(_settings.Encryption.ProductSalt));

    var payload = new LicensePayload(
      Guid.NewGuid(),
      "Demo License Customer",
      fingerPrint,
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
      DateTime.UtcNow,
      DateTime.UtcNow.AddDays(_settings.DemoLicense.DurationInDays), // Demo license valid days based on nHardware count
      _settings.DemoLicense.GracePeriodInDays,
      false
    );

    var json = JsonSerializer.Serialize(payload);
    var data = Encoding.UTF8.GetBytes(json);

    var encrypt = CryptoHelper.Encrypt(data, aesKey);
    var (iv, tag, cipher) = CryptoHelper.Unpack(encrypt);
    var signedBytes = iv.Concat(cipher).Concat(tag).ToArray();

    using var rsa = RSA.Create();
    string pem = Encoding.UTF8.GetString(keys.private_key);
    rsa.ImportFromPem(pem);
    var sig = rsa.SignData(signedBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

    var license = new EncryptedLicense(
      1,
      keys.key_uuid,
      Convert.ToBase64String(encrypt),
      Convert.ToBase64String(sig),
      Convert.ToBase64String(keys.public_key)
    );

    File.WriteAllText("license.json", JsonSerializer.Serialize(license, new JsonSerializerOptions { WriteIndented = true }));
    Console.WriteLine("License generated.");

    return license;
  }
}
