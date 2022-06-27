var stopwatch = new Stopwatch();
stopwatch.Start();

Console.WriteLine($"{DateTime.Now.ToLocalDEDateTime()}- Starting API ...");
Console.WriteLine();

// Add services to the container
var builder = WebApplication.CreateBuilder(args);
builder = builder.ConfigureApplication();    

var app = builder.Build().ConfigureHttpPipeline();
stopwatch.Stop();

Console.WriteLine();
var startupCompletionTime = DateTime.Now;
Log.Information("{ApplicationStartupCompletionTime}- API Startup completed in {ApplicationStartupDuration} milliseconds ...", startupCompletionTime.ToLocalDEDateTime(), stopwatch.ElapsedMilliseconds);
app.Run();