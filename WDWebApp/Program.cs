using WDWebApp;


var builder = WebApplication.CreateBuilder(args);

var hostBuilder = builder.Host.UseWindowsService();
hostBuilder.ConfigureServices(services =>{services.AddHostedService<Worker2>();});


var app = builder.Build();

// var serviceCollection = new ServiceCollection();
// var serviceProvider = serviceCollection.BuildServiceProvider();

// app.MapControllers();
app.MapGet("/", () => "Hello World!");

app.MapGet("/start", () =>
{
    // var worker = serviceProvider.GetRequiredService<Worker>();
    // worker.StartAsync(new CancellationTokenSource().Token);
    
    
    return "start!";
});

app.MapGet("/stop", () =>
{
    // var worker = serviceProvider.GetRequiredService<Worker>();
    // worker.StopAsync(new CancellationTokenSource().Token);
    using (var scope = app.Services.CreateScope())
    {
        var worker = scope.ServiceProvider.GetRequiredService<IHostedService>();
        worker.StopAsync(new CancellationTokenSource().Token);
    }

    Console.WriteLine("call stop");
    return "stop!";
});


var port = Environment.GetEnvironmentVariable("PORT") ?? "6666";

app.Urls.Add("https://localhost:8888");
app.Urls.Add("https://localhost:7203");

// app.Run($"http://localhost:{port}");
app.Run();