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
    var key = await context.sign_key
      .OrderByDescending(k => k.created_date)
      .Where(x => !x.is_revoked)
      .FirstOrDefaultAsync();

    if (key == null)
    {
      // Rotate key
      Console.WriteLine("Rotating keys...");

      // Generate new key
      var signer = EncryptHelper.CreateSigner();
      var en = new SignKeyAudit
      {
        sign_key_uuid = Guid.NewGuid(),
        sign_pub = signer.ExportSubjectPublicKeyInfo(),
        sign_priv = signer.ExportPkcs8PrivateKey(),
        created_date = DateTime.UtcNow,
        expire_date = DateTime.UtcNow.AddYears(1),
        is_revoked = false
      };

      await context.sign_key.AddAsync(en);
      await context.SaveChangesAsync();

      Console.WriteLine("Key rotation completed.");
      return;

    }

    if (key.expire_date <= DateTime.UtcNow)
    {
      // Rotate key
      Console.WriteLine("Rotating keys...");

      key.is_revoked = true;
      context.sign_key.Update(key);

      var signer = EncryptHelper.CreateSigner();
      var en = new SignKeyAudit
      {
        sign_key_uuid = Guid.NewGuid(),
        sign_pub = signer.ExportSubjectPublicKeyInfo(),
        sign_priv = signer.ExportPkcs8PrivateKey(),
        created_date = DateTime.UtcNow,
        expire_date = DateTime.UtcNow.AddYears(1),
        is_revoked = false
      };

      await context.sign_key.AddAsync(en);
      await context.SaveChangesAsync();

      Console.WriteLine("Key rotation completed.");
    }
  }
}
