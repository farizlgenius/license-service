using System;

namespace LicenseService.Model;

public sealed record VerifyRequest(string sessionId, string signature);
