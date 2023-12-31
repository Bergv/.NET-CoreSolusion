namespace WDWebApp;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private bool _isStopping = false; //是否正在停止工作;

    public Worker(ILogger<Worker> logger, IHostApplicationLifetime hostApplicationLifetime)
    {
        _logger = logger;
        _hostApplicationLifetime = hostApplicationLifetime;
    }
    
    public override Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("上班了，又是精神抖擞的一天，output from StartAsync");
        return base.StartAsync(cancellationToken);
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
 
                    await SomeMethodThatDoesTheWork(stoppingToken);
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
            GetOffWork(stoppingToken); //关闭前需要完成的工作
            _hostApplicationLifetime.StopApplication(); //手动调用 StopApplication
        }
    }
    
    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("太好了，下班时间到了，output from StopAsync at: {time}", DateTimeOffset.Now);
 
        _isStopping = true;
 
        _logger.LogInformation("去洗洗茶杯先……", DateTimeOffset.Now);
        Task.Delay(3000).Wait();
        _logger.LogInformation("茶杯洗好了。", DateTimeOffset.Now);
 
        _logger.LogInformation("下班喽 ^_^", DateTimeOffset.Now);
 
        return base.StopAsync(cancellationToken);
    }

    private async Task SomeMethodThatDoesTheWork(CancellationToken cancellationToken)
    {
        if (_isStopping)
            _logger.LogInformation("假装还在埋头苦干ing…… 其实我去洗杯子了");
        else
            _logger.LogInformation("我爱工作，埋头苦干ing……");
 
        await Task.CompletedTask;
    }
    
 
    /// <summary>
    /// 关闭前需要完成的工作
    /// </summary>
    private void GetOffWork(CancellationToken stoppingToken)
    {
        _logger.LogInformation("啊，糟糕，有一个紧急 bug 需要下班前完成！！！");
 
        _logger.LogInformation("啊啊啊，我爱加班，我要再干 20 秒，Wait 1 ");
 
        Task.Delay(TimeSpan.FromSeconds(20)).Wait();
 
        _logger.LogInformation("啊啊啊啊啊啊，我爱加班，我要再干 1 分钟，Wait 2 ");
 
        Task.Delay(TimeSpan.FromSeconds(10)).Wait();
 
        _logger.LogInformation("啊哈哈哈哈哈，终于好了，下班走人！");
    }
}