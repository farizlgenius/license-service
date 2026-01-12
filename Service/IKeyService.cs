using System;
using LicenseService.Entity;

namespace LicenseService.Service;

public interface IKeyService
{
  Task GenerateKey();
}
