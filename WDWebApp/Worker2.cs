namespace WDWebApp;

public class Worker2 : BackgroundService
{
    private readonly ILogger<Worker2> _logger;

    public Worker2(ILogger<Worker2> logger)
    {
        _logger = logger;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                // await Task.Delay(1000, stoppingToken);
                
                // await Task.Delay(1000, stoppingToken).ContinueWith(tsk =>
                // {
                //     Console.WriteLine(string.Format("{0} working...", DateTime.Now.ToString("hh:mm:ss")));
                // });
                
                try
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
 
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Global exception occurred. Will resume in a moment.");
                }
 
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }
        finally
        {
            _logger.LogWarning("Exiting application...");
            Console.WriteLine("关闭前需要完成的工作......");
            await Task.Delay(TimeSpan.FromSeconds(10));
            Console.WriteLine("关闭前需要完成的工作--OK.");
        }
    }
    
    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("call StopAsync at: {time}", DateTimeOffset.Now);
        return base.StopAsync(cancellationToken);
    }
    

}