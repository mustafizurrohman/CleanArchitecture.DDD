// Add services to the container
var builder = WebApplication.CreateBuilder(args);
builder = builder.ConfigureApplication();    

var app = builder.Build().ConfigureHttpPipeline();
app.Run();