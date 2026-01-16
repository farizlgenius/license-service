using System;

namespace LicenseService.Model;

public sealed record GenerateDemo(string sessionId,string company, string customerSite,string machineId);
