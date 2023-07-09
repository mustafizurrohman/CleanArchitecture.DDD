using CleanArchitecture.DDD.API.Startup;

var stopwatch = new Stopwatch();
stopwatch.Start();

var startupTime = DateTime.Now;
LoggingHelper.LogWithSpace(() =>
{
    Console.WriteLine($"[{startupTime:HH.mm:ss} INF] Starting API ...");
});

// Add services to the container
var builder = WebApplication.CreateBuilder(args);
builder = builder.ConfigureApplication();    

var app = builder.Build().ConfigureHttpPipeline();
stopwatch.Stop();

LoggingHelper.LogWithSpace(() =>
{
    Log.Information("API Startup completed in {ApplicationStartupDuration} milliseconds ...", stopwatch.ElapsedMilliseconds);
});

app.Run();