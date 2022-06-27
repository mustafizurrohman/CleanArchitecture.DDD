var stopwatch = new Stopwatch();
stopwatch.Start();

Console.WriteLine($"{DateTime.Now.ToLocalDateTime()}- Starting API ...");
Console.WriteLine();

// Add services to the container
var builder = WebApplication.CreateBuilder(args);
builder = builder.ConfigureApplication();    

var app = builder.Build().ConfigureHttpPipeline();
stopwatch.Stop();

Console.WriteLine();
Console.WriteLine($"{DateTime.Now.ToLocalDateTime()}- API Startup completed in {stopwatch.ElapsedMilliseconds} milliseconds ...");
app.Run();