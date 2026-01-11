using LicenseService.Data;
using LicenseService.Service;
using Microsoft.EntityFrameworkCore;

namespace LicenseService.Worker;

public class StartupTask : IHostedService
{
  private readonly IServiceScopeFactory _scopeFactory;

  public StartupTask(IServiceScopeFactory scopeFactory)
  {
    _scopeFactory = scopeFactory;
  }

  public async Task StartAsync(CancellationToken cancellationToken)
  {
    await RunOnStartupAsync(cancellationToken);
  }

  public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

  private async Task RunOnStartupAsync(CancellationToken cancellationToken)
  {
    Console.WriteLine("Startup function executed");

    using var scope = _scopeFactory.CreateScope();

    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var keyRotateService = scope.ServiceProvider.GetRequiredService<IKeyService>();

    var hasKey = await context.Keys.AnyAsync(x => !x.is_revoked, cancellationToken);

    if (!hasKey)
    {
      await keyRotateService.GenerateKey();
    }
  }
}