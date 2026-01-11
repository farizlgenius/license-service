using System;

namespace LicenseService.Model;

public record EncryptedLicense(
    int Version,
    string KeyId,
    string Enc,
    string Sig,
    string Pub
);