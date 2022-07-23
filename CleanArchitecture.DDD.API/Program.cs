using CleanArchitecture.DDD.API.Startup;

var stopwatch = new Stopwatch();
stopwatch.Start();

var startupTime = DateTime.Now;
Console.WriteLine($"[{startupTime:HH.mm:ss} INF] Starting API ...");
Console.WriteLine();

// Add services to the container
var builder = WebApplication.CreateBuilder(args);
builder = builder.ConfigureApplication();    

var app = builder.Build().ConfigureHttpPipeline();
stopwatch.Stop();

Console.WriteLine();
Log.Information("API Startup completed in {ApplicationStartupDuration} milliseconds ...", stopwatch.ElapsedMilliseconds);
Console.WriteLine();
app.Run();