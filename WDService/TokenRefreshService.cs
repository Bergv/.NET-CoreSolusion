namespace WDService;

public class TokenRefreshService : IHostedService, IDisposable
{
    private readonly ILogger _logger;
    private Timer _timer;

    public TokenRefreshService(ILogger<TokenRefreshService> logger)
    {
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Service starting");
        _timer = new Timer(Refresh, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
        return Task.CompletedTask;
    }

    private void Refresh(object state)
    {
        _logger.LogInformation(DateTime.Now.ToLongTimeString() + ": Refresh Token!"); //在此写需要执行的任务
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Service stopping");
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}