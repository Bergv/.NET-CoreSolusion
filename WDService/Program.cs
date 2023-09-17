using WDService;

IHostBuilder builder = Host.CreateDefaultBuilder(args);

// builder.UseWindowsService();//指定项目可以部署为Windows服务
builder.UseWindowsService(options => options.ServiceName = ".NET Joke Service");


// builder.ConfigureServices(services => { services.AddHostedService<Worker>(); });
builder.ConfigureServices(services =>
{
    services.AddSingleton<JokeService>();
    services.AddHostedService<JokeBackgroundService>();
});
// builder.ConfigureServices(services => { services.AddSingleton<IHostedService, TokenRefreshService>(); });



IHost host = builder.Build();

await host.RunAsync();