using System;

namespace LicenseService.Service;

public interface IRedisService
{
    Task<string?> GetAsync(string key);
      Task<bool> SetAsync(string key, string value, TimeSpan? expiry = null);
      Task<bool> KeyExistsAsync(string key);
      Task<bool> DeleteAsync(string key);
}
