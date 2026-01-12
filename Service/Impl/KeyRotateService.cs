using System;
using LicenseService.Data;
using LicenseService.Entity;
using LicenseService.Helper;
using LicenseService.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace LicenseService.Service.Impl;

public sealed class KeyRotateService(AppDbContext context, IOptions<AppConfigSetting> options) : IKeyRotateService
{
  private readonly AppConfigSetting _settings = options.Value;

  public async Task CheckRotateNeededAsync()
  {
    var key = await context.KeyPairs
      .OrderByDescending(k => k.created_date)
      .Where(x => !x.is_revoked)
      .FirstOrDefaultAsync();

    if (key == null)
    {
      // Rotate key
      Console.WriteLine("Rotating keys...");

      // Generate new key
      var (publicKey, privateKey) = EcdhCryptoHelper.GenerateEcdhKeyPair();

      var newKey = new ECDHKeyPair
      {
        key_uuid = Guid.NewGuid(),
        public_key = publicKey,
        secret_key = privateKey,
        created_date = DateTime.UtcNow,
        expire_date = DateTime.UtcNow.AddMonths(_settings.Encryption.KeyExpireInMonth),
        is_revoked = false,
      };

      await context.KeyPairs.AddAsync(newKey);

      await context.SaveChangesAsync();

      Console.WriteLine("Key rotation completed.");
      return;

    }

    if (key.expire_date.ToLocalTime() <= DateTime.Now)
    {
      // Rotate key
      Console.WriteLine("Rotating keys...");

      key.is_revoked = true;
      context.KeyPairs.Update(key);

      // Generate new key
      var (publicKey, privateKey) = EcdhCryptoHelper.GenerateEcdhKeyPair();


      var newKey = new Entity.ECDHKeyPair
      {
        key_uuid = Guid.NewGuid(),
        public_key = publicKey,
        secret_key = privateKey,
        created_date = DateTime.UtcNow,
        expire_date = DateTime.UtcNow.AddMonths(_settings.Encryption.KeyExpireInMonth),
        is_revoked = false,
      };

      await context.KeyPairs.AddAsync(newKey);

      await context.SaveChangesAsync();

      Console.WriteLine("Key rotation completed.");
    }
  }
}
