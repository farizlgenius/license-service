using System;
using LicenseService.Model;

namespace LicenseService.Service.Impl;

public class LicenseService : ILicenseService
{
  public Task<EncryptedLicenseDto> CreateLicenseAsync(LicensePayload payload)
  {
    throw new NotImplementedException();
  }

  public Task<EncryptedLicenseDto> CreateLicenseDemoAsync()
  {
    throw new NotImplementedException();
  }
}
