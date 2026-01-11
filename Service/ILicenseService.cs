using System;
using LicenseService.Model;

namespace LicenseService.Service.Impl;

public interface ILicenseService
{
  Task<EncryptedLicenseDto> CreateLicenseDemoAsync();
  Task<EncryptedLicenseDto> CreateLicenseAsync(LicensePayload payload);
}
