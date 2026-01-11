using System;
using System.Security.Cryptography;
using LicenseService.Data;
using LicenseService.Entity;

namespace LicenseService.Service.Impl;

public sealed class KeyService(AppDbContext context) : IKeyService
{
  public async Task<KeyPair> GenerateKey()
  {
    using var rsa = RSA.Create(4096);

    string privateKeyPem = rsa.ExportRSAPrivateKeyPem();
    string publicKeyPem = rsa.ExportRSAPublicKeyPem();

    var keyPair = new KeyPair
    {
      KeyId = Guid.NewGuid().ToString(),
      PrivateKey = privateKeyPem,
      PublicKey = publicKeyPem,
      CreatedDate = DateTime.Now,
      IsRevoked = false
    };

    await context.Keys.AddAsync(keyPair);
    await context.SaveChangesAsync();

    File.WriteAllText("license_private.pem", privateKeyPem);
    File.WriteAllText("license_public.pem", publicKeyPem);

    Console.WriteLine("Keys generated.");

    return keyPair;


  }
}
