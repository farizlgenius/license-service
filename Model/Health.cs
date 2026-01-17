using System;

namespace LicenseService.Model;

public sealed record HealthDto(string Status, DateTime Uptime);
