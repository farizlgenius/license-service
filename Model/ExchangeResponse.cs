using System;

namespace LicenseService.Model;

public sealed record ExchangeResponse(string sessionId, string dhPub, string signPub, string signature);
