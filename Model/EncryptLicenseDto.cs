using System;

namespace LicenseService.Model;

public record EncryptedLicenseDto(
    int Version,
    string KeyId,
    string Iv,
    string Cipher,
    string Tag,
    LicensePayload Payload,
    string Signature
);