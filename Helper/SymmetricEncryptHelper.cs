using System;
using System.Security.Cryptography;

namespace LicenseService.Helper;

public static class SymmetricEncryptHelper
{
  private const int IvSize = 12;   // 96-bit nonce (recommended for GCM)
  private const int TagSize = 16;  // 128-bit auth tag

  public static byte[] Encrypt(
      byte[] data,
      byte[] key,
      byte[]? associatedData = null)
  {
    ValidateKey(key);

    var iv = RandomNumberGenerator.GetBytes(IvSize);
    var cipher = new byte[data.Length];
    var tag = new byte[TagSize];

    using var aes = new AesGcm(key);
    aes.Encrypt(iv, data, cipher, tag, associatedData);

    return Pack(iv, tag, cipher);
  }

  public static byte[] Decrypt(
      byte[] packed,
      byte[] key,
      byte[]? associatedData = null)
  {
    ValidateKey(key);

    var (iv, tag, cipher) = Unpack(packed);

    var data = new byte[cipher.Length];

    using var aes = new AesGcm(key);
    aes.Decrypt(iv, cipher, tag, data, associatedData);

    return data;
  }

  // ----------------- helpers -----------------

  private static void ValidateKey(byte[] key)
  {
    if (key is null)
      throw new ArgumentNullException(nameof(key));

    if (key.Length != 16 && key.Length != 24 && key.Length != 32)
      throw new ArgumentException("Key must be 128, 192, or 256 bits.", nameof(key));
  }

  private static byte[] Pack(byte[] iv, byte[] tag, byte[] cipher)
  {
    var result = new byte[IvSize + TagSize + cipher.Length];

    Buffer.BlockCopy(iv, 0, result, 0, IvSize);
    Buffer.BlockCopy(tag, 0, result, IvSize, TagSize);
    Buffer.BlockCopy(cipher, 0, result, IvSize + TagSize, cipher.Length);

    return result;
  }

  public static (byte[] iv, byte[] tag, byte[] cipher) Unpack(byte[] packed)
  {
    if (packed.Length < IvSize + TagSize)
      throw new ArgumentException("Invalid encrypted payload.", nameof(packed));

    var iv = new byte[IvSize];
    var tag = new byte[TagSize];
    var cipher = new byte[packed.Length - IvSize - TagSize];

    Buffer.BlockCopy(packed, 0, iv, 0, IvSize);
    Buffer.BlockCopy(packed, IvSize, tag, 0, TagSize);
    Buffer.BlockCopy(packed, IvSize + TagSize, cipher, 0, cipher.Length);

    return (iv, tag, cipher);
  }

  public static (byte[] PublicKey, byte[] PrivateKey) GenerateRsaKeyPair()
  {
    using var rsa = RSA.Create(4096);
    var publicKey = rsa.ExportSubjectPublicKeyInfo();
    var privateKey = rsa.ExportPkcs8PrivateKey();
    return (publicKey, privateKey);
  }
}