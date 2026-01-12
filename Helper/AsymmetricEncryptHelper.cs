using System;
using System.Security.Cryptography;
using LicenseService.Model;

namespace LicenseService.Helper;

public static class AsymmetricEncryptHelper
{
//   var publicKeyPem = File.ReadAllText("license_public.pem");
// var aesKey = SHA256.HashData(Encoding.UTF8.GetBytes("PRODUCT-SALT"));
// var fingerprint = MachineFingerprint.Get();

// var license = JsonSerializer.Deserialize<EncryptedLicenseFile>(File.ReadAllText("license.json"))!;
// var payload = LicenseValidator.Validate(license, publicKeyPem, aesKey, fingerprint);

// Console.WriteLine($"Activated for {payload.Customer}");

    // public static LicensePayload Validate(
    //     EncryptedLicenseFile file,
    //     string publicKeyPem,
    //     byte[] aesKey,
    //     string localFingerprint)
    // {
    //     var iv = Convert.FromBase64String(file.Iv);
    //     var cipher = Convert.FromBase64String(file.Cipher);
    //     var tag = Convert.FromBase64String(file.Tag);
    //     var sig = Convert.FromBase64String(file.Signature);

    //     var signedBytes = iv.Concat(cipher).Concat(tag).ToArray();

    //     using var rsa = RSA.Create();
    //     rsa.ImportFromPem(publicKeyPem);

    //     if (!rsa.VerifyData(signedBytes, sig, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1))
    //         throw new Exception("Invalid license signature");

    //     var json = Encoding.UTF8.GetString(Crypto.Decrypt(cipher, iv, tag, aesKey));
    //     var payload = JsonSerializer.Deserialize<LicensePayload>(json)!;

    //     if (payload.ExpiresUtc < DateTime.UtcNow)
    //         throw new Exception("License expired");

    //     if (payload.MachineFingerprint != localFingerprint)
    //         throw new Exception("License not valid for this machine");

    //     return payload;
    // }


  public static (byte[] PublicKey, byte[] PrivateKey) GenerateRsaKeyPair()
  {
    using var rsa = RSA.Create(4096);
    var publicKey = rsa.ExportSubjectPublicKeyInfo();
    var privateKey = rsa.ExportPkcs8PrivateKey();
    return (publicKey, privateKey);
  }

}