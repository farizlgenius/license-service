using LicenseService.Service;

namespace LicenseService.Worker;

public sealed class KeyRotationWorker : BackgroundService
{
  private readonly IServiceScopeFactory _scopeFactory;

  public KeyRotationWorker(IServiceScopeFactory scopeFactory)
  {
    _scopeFactory = scopeFactory;
  }

  protected override async Task ExecuteAsync(CancellationToken stoppingToken)
  {
    while (!stoppingToken.IsCancellationRequested)
    {
      using var scope = _scopeFactory.CreateScope();
      var service = scope.ServiceProvider.GetRequiredService<IKeyRotateService>();

      await service.CheckRotateNeededAsync();

      await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
    }
  }
}