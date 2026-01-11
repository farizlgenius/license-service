using System;
using LicenseService.Service;

namespace LicenseService.Worker;

public sealed class KeyRotationWorker(IKeyRotateService service) : BackgroundService
{
  protected async override Task ExecuteAsync(CancellationToken stoppingToken)
  {
    while (!stoppingToken.IsCancellationRequested)
    {
      await service.CheckRotateNeededAsync();

      // Check once per day
      await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
    }
  }
}
