using System;

namespace LicenseService.Service;

public interface IKeyRotateService
{
  Task CheckRotateNeededAsync();
}
