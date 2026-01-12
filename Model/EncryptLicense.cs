using System;

namespace LicenseService.Model;

public record EncryptedLicense(
    Guid KeyUuid,
    string Packed,
    string Sig
);