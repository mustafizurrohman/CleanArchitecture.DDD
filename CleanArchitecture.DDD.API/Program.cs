using CleanArchitecture.DDD.API.ExtensionMethods;
using CleanArchitecture.DDD.Core;
using CleanArchitecture.DDD.Core.ExtensionMethods;
using CleanArchitecture.DDD.Domain.ValueObjects;
using CleanArchitecture.DDD.Infrastructure.Persistence.DbContext;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;

// Add services to the container
var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DDD_Db");
builder.Services.AddDbContext<DomainDbContext>(options =>
{
    options.UseSqlServer(connectionString);
    #if DEBUG
    options.EnableSensitiveDataLogging()
        .UseLoggerFactory(LoggerFactory.Create(loggerBuilder =>
        {
            loggerBuilder.AddConsole();
        }));
    #endif
});

builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IValidator<Name>, NameValidator>();
builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CoreAssemblyMarker>());
builder.Services.AddValidatorsFromAssemblies(new[] { typeof(CoreAssemblyMarker).Assembly });

    
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MigrateDatabase();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
