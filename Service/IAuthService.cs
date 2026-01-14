using System;
using LicenseService.Model;

namespace LicenseService.Service;

public interface IAuthService
{
      Task<ExchangeResponse> ExchangeAsync(ExchangeRequest request);
      Task<bool> VerifyAsync(VerifyRequest request);
}
