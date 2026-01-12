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
  public async Task GenerateKey()
  {
    var (publicKey, privateKey) = Helper.EcdhCryptoHelper.GenerateEcdhKeyPair();

    var newKey = new Entity.ECDHKeyPair
    {
      key_uuid = Guid.NewGuid(),
      public_key = publicKey,
      secret_key = privateKey,
      created_date = DateTime.UtcNow,
      expire_date = DateTime.UtcNow.AddMonths(_settings.Encryption.KeyExpireInMonth),
      is_revoked = false
    };

    await context.KeyPairs.AddAsync(newKey);
    await context.SaveChangesAsync();
  }
}
