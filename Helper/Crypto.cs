using System;
using System.Security.Cryptography;

namespace LicenseService.Helper;

public static class Crypto
{
  public static (byte[] Cipher, byte[] Iv, byte[] Tag) Encrypt(byte[] data, byte[] key)
  {
    var iv = RandomNumberGenerator.GetBytes(12);
    var cipher = new byte[data.Length];
    var tag = new byte[16];

    using var aes = new AesGcm(key);
    aes.Encrypt(iv, data, cipher, tag);

    return (cipher, iv, tag);
  }

  public static byte[] Decrypt(byte[] cipher, byte[] iv, byte[] tag, byte[] key)
  {
    var data = new byte[cipher.Length];
    using var aes = new AesGcm(key);
    aes.Decrypt(iv, cipher, tag, data);
    return data;
  }
}
