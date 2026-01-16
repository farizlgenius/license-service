using System;
using System.Security.Cryptography;
using System.Text;

namespace LicenseService.Helper;

public static class EncryptHelper
{
      /// <summary>
      /// Generates ECDH key pair for key exchange.
      /// </summary>
      /// <returns>The ECDH key pair.</returns>
      public static ECDiffieHellman CreateDh() => ECDiffieHellman.Create(ECCurve.NamedCurves.nistP256);

      /// <summary>
      /// Generate ECDSA key pair for signing.
      /// </summary>
      /// <returns>The ECDSA key pair.</returns
      public static ECDsa CreateSigner() => ECDsa.Create(ECCurve.NamedCurves.nistP256);

      /// <summary>
      /// Exports the public key from the ECDH key pair.
      /// </summary>
      /// <param name="dh">The ECDH key pair.</param>
      /// <returns>The public key bytes.</returns>
      public static byte[] ExportDhPublicKey(ECDiffieHellman dh) => dh.ExportSubjectPublicKeyInfo();

      /// <summary>
      /// Exports the public key from the ECDSA key pair.
      /// </summary>
      /// <param name="signer">The ECDSA key pair.</param>
      /// <returns>The public key bytes.</returns>
      public static byte[] ExportSignerPublicKey(ECDsa signer) => signer.ExportSubjectPublicKeyInfo();

      /// <summary>
      /// Imports the ECDSA public key.
      /// </summary>
      /// <param name="privateKey">The private key bytes.</param>
      /// <returns>The ECDSA key pair.</returns>
      public static ECDsa LoadSignerPrivateKey(byte[] privateKey)
      {
            var ecdsa = ECDsa.Create(ECCurve.NamedCurves.nistP256);
            ecdsa.ImportPkcs8PrivateKey(privateKey, out _);
            return ecdsa;
      }

      /// <summary>
      /// Imports the ECDSA public key.
      /// </summary>
      /// <param name="publicKey">The public key bytes.</param>
      /// <returns>The ECDSA key pair.</returns>
      public static ECDsa LoadVerifierPublicKey(byte[] publicKey)
      {
            var ecdsa = ECDsa.Create(ECCurve.NamedCurves.nistP256);
            ecdsa.ImportSubjectPublicKeyInfo(publicKey, out _);
            return ecdsa;
      }

      /// <summary>
      /// Imports the ECDH private key.
      /// </summary>
      /// <param name="privateKey"></param>
      /// <returns></returns>
      public static ECDiffieHellman LoadDhPrivateKey(byte[] privateKey)
      {
            var dh = ECDiffieHellman.Create(ECCurve.NamedCurves.nistP256);
            dh.ImportPkcs8PrivateKey(privateKey, out _);
            return dh;
      }


      /// <summary>
      /// Derives the shared secret key using ECDH key exchange.
      /// </summary>
      /// <param name="dh">The ECDiffieHellman instance.</param>
      /// <param name="otherPub">The peer public key bytes.</param>
      /// <returns>The derived secret key bytes.</returns>
      public static byte[] DeriveSecretKey(ECDiffieHellman dh, byte[] otherPub)
      {
            using var other = ECDiffieHellman.Create(ECCurve.NamedCurves.nistP256);
            other.ImportSubjectPublicKeyInfo(otherPub, out _);
            return dh.DeriveKeyMaterial(other.PublicKey);
      }

      /// <summary>
      /// Signs the data using ECDSA.
      /// </summary>
      /// <param name="signer">The ECDsa instance.</param>
      /// <param name="data">The data to sign.</param>
      /// <returns>The signature bytes.</returns>
      public static byte[] SignData(ECDsa signer, byte[] data)
      {
            return signer.SignData(data, HashAlgorithmName.SHA256);
      }

      /// <summary>
      /// Verifies the data signature using ECDSA.
      /// </summary>
      /// <param name="data">The data to verify.</param>
      /// <param name="signature">The signature bytes.</param>
      /// <param name="pubickey">The public key bytes.</param>
      /// <returns>True if the signature is valid, false otherwise.</returns>
      public static bool VerifyData(byte[] data, byte[] signature,byte[] pubickey)
      {
            using var ecdsa = ECDsa.Create(ECCurve.NamedCurves.nistP256);
            ecdsa.ImportSubjectPublicKeyInfo(pubickey,out _);
            return ecdsa.VerifyData(data, signature, HashAlgorithmName.SHA256);
      }

      /// <summary>
      /// Encrypts the data using AES-GCM.
      /// </summary>
      /// <param name="key">The AES key.</param>
      /// <param name="data">The data to encrypt.</param>
      /// <returns>The encrypted data.</returns>
      public static byte[] EncryptAes(byte[] key,byte[] data)
      {
            var iv = RandomNumberGenerator.GetBytes(12);
            var cipher = new byte[data.Length];
            var tag = new byte[16];

            using var aes = new AesGcm(key,tagSizeInBytes:16);
            aes.Encrypt(iv, data, cipher, tag);
            
            return iv.Concat(tag).Concat(cipher).ToArray();
      }

      /// <summary>
      /// Decrypts the data using AES-GCM.
      /// </summary>
      /// <param name="key">The AES key.</param>
      /// <param name="encryptedData">The encrypted data.</param>
      /// <returns>The decrypted data.</returns>
      public static byte[] DecryptAes(byte[] key,byte[] encryptedData)
      {
            var iv = encryptedData[..12];
            var tag = encryptedData[12..28];
            var cipher = encryptedData[28..];

            var plain = new byte[cipher.Length];

            using var aes = new AesGcm(key,tagSizeInBytes:16);
            aes.Decrypt(iv, cipher, tag, plain);

            return plain;
      }

      /// <summary>
      /// Derives AES key from shared secret using HKDF.
      /// </summary>
      /// <param name="sharedSecret"></param>
      /// <returns></returns>
      public static byte[] DeriveAesKey(byte[] sharedSecret,string info)
      {
            return HKDF.DeriveKey(
                HashAlgorithmName.SHA256,
                sharedSecret,
                32,
                salt: null,
                info: Encoding.UTF8.GetBytes(info)
            );
      }

      /// <summary>
      /// Builds the payload by combining license and signature.
      /// </summary>
      /// <param name="license"></param>
      /// <param name="signature"></param>
      /// <returns></returns>
      public static byte[] BuildPayload(byte[] license, byte[] signature)
      {
            using var ms = new MemoryStream();
            using var bw = new BinaryWriter(ms);

            bw.Write(license.Length);
            bw.Write(license);
            bw.Write(signature.Length);
            bw.Write(signature);

            return ms.ToArray();
      }


}
