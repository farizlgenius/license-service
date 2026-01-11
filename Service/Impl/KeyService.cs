using System;
using System.Security.Cryptography;
using LicenseService.Data;
using LicenseService.Entity;
using LicenseService.Model;
using Microsoft.Extensions.Options;

namespace LicenseService.Service.Impl;

public sealed class KeyService(AppDbContext context, IOptions<AppConfigSetting> options) : IKeyService
{
  private readonly AppConfigSetting _settings = options.Value;
  public async Task<KeyPair> GenerateKey()
  {
    var (pub, pri) = Helper.CryptoHelper.GenerateRsaKeyPair();

    var newKey = new Entity.KeyPair
    {
      key_uuid = Guid.NewGuid().ToString(),
      private_key = Helper.CryptoHelper.Encrypt(pri, System.Text.Encoding.UTF8.GetBytes(_settings.Encryption.ProductSalt)),
      public_key = pub,
      created_date = DateTime.Now,
      expire_date = DateTime.Now.AddMonths(6),
      is_revoked = false
    };

    await context.Keys.AddAsync(newKey);
    await context.SaveChangesAsync();

    return newKey;
  }
}
