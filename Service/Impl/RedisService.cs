using System;
using StackExchange.Redis;

namespace LicenseService.Service.Impl;

public class RedisService(IDatabase redis) : IRedisService
{
  public Task DeleteAsync(string key)
  {
    throw new NotImplementedException();
  }

  public async Task<string?> GetAsync(string key)
  {
    return await redis.StringGetAsync(key);
  }

  public async Task<bool> KeyExistsAsync(string key)
  {
    return await redis.KeyExistsAsync(key);
  }

  public async Task SetAsync(string key, string value, TimeSpan? expiry = null)
  {
    await redis.StringSetAsync(key, value, DateTime.UtcNow.Add(expiry ?? TimeSpan.FromHours(1)));
  }
}

