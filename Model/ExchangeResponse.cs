using System;

namespace LicenseService.Model;

public sealed record ExchangeResponse(string dhPub, string signPub,string signature);
