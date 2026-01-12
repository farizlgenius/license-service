using System;

namespace LicenseService.Model;

public sealed record TrustServerDto(string machineId, string peerPublicKey);
