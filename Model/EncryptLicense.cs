using System;

namespace LicenseService.Model;

public record EncryptedLicense(
    string SessionId,
    string Payload,
    string Signature,
    string ServerSingPublic
);