using System;

namespace LicenseService.Service;

public interface IRedisService
{
  Task<string?> GetAsync(string key);
  Task SetAsync(string key, string value, TimeSpan? expiry = null);
  Task<bool> KeyExistsAsync(string key);
  Task DeleteAsync(string key);
}
