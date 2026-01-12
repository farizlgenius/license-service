using System;
using System.Security.Cryptography;

namespace LicenseService.Helper;

public static class EcdhCryptoHelper
{
    // Step 1 : Generate ECDH key pair
    public static (byte[] publicKey, byte[] privateKey) GenerateEcdhKeyPair()
    {
        using var ecdh = ECDiffieHellman.Create(ECCurve.NamedCurves.nistP256);
        var publicKey = ecdh.ExportSubjectPublicKeyInfo();
        var privateKey = ecdh.ExportPkcs8PrivateKey();
        return (publicKey, privateKey);
    }

    // Step 2 : Derive shared secret
    public static byte[] DeriveSharedSecret(byte[] privateKey, byte[] peerPublicKey)
    {
        using var ecdh = ECDiffieHellman.Create();
        ecdh.ImportPkcs8PrivateKey(privateKey, out _);
        using var peerEcdh = ECDiffieHellman.Create();
        peerEcdh.ImportSubjectPublicKeyInfo(peerPublicKey, out _);
        return ecdh.DeriveKeyMaterial(peerEcdh.PublicKey);
    }
}
