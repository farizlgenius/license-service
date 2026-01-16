namespace LicenseService.Model;

public sealed record ExchangeRequest(string sessionId,string appDhPublic,string appSignPublic,string signature);
