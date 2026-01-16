using System;
using LicenseService.Model;

namespace LicenseService.Service;

public interface IAuthService
{
      Task<BaseDto<ExchangeResponse>> ExchangeAsync(ExchangeRequest request);
}
