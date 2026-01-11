using System;
using LicenseService.Data;
using LicenseService.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace LicenseService.Service.Impl;

public sealed class KeyRotateService : IKeyRotateService
{
  private readonly AppDbContext _context;
  private readonly AppConfigSetting _settings;
  public KeyRotateService(AppDbContext context, IOptions<AppConfigSetting> options)
  {
    _context = context;
    _settings = options.Value;
  }
  public async Task CheckRotateNeededAsync()
  {
    var key = await _context.Keys
      .OrderByDescending(k => k.created_date)
      .Where(x => !x.is_revoked)
      .FirstOrDefaultAsync();

    if (key == null || key.expire_date <= DateTime.Now)
    {
      // Rotate key
      Console.WriteLine("Rotating keys...");

      // Revoke old key
      if (key != null)
      {
        key.is_revoked = true;
        _context.Keys.Update(key);
      }

      // Generate new key
      var (pub, pri) = Helper.CryptoHelper.GenerateRsaKeyPair();

      Console.WriteLine(_settings.Encryption.ProductSalt);

      var newKey = new Entity.KeyPair
      {
        key_uuid = Guid.NewGuid().ToString(),
        private_key = Helper.CryptoHelper.Encrypt(pri, Convert.FromBase64String(_settings.Encryption.ProductSalt)),
        public_key = pub,
        created_date = DateTime.Now,
        expire_date = DateTime.Now.AddMonths(6),
        is_revoked = false
      };

      await _context.Keys.AddAsync(newKey);

      await _context.SaveChangesAsync();

      Console.WriteLine("Key rotation completed.");
    }
  }
}
