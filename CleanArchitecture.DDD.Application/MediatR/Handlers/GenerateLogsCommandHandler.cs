using System.Security.Cryptography;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.DDD.Application.MediatR.Handlers;

public class GenerateLogsCommandHandler : BaseHandler, IRequestHandler<GenerateLogsCommand>
{
    private readonly Faker _faker;

    private static int RandomDelay => RandomNumberGenerator.GetInt32(500, 1000);
    private string RandomText => _faker.Lorem.Lines(1);

    public GenerateLogsCommandHandler(IAppServices appServices)
        : base(appServices)
    {
        _faker = new Faker();
    }


    public Task Handle(GenerateLogsCommand request, CancellationToken cancellationToken)
    {
        for (var i = 0; i < request.Iterations; i++)
        {
            GenerateLogs(request.WithDelay);

            if (request.WithDelay)
                Thread.Sleep((i+1) * RandomDelay);
        }

        return Task.CompletedTask;
    }

    private void GenerateLogs(bool withDelay)
    {
        IEnumerable<Action> GetSetOfLogs()
        {
            Action GetLog(LogLevel logLevel)
            {
                var withParams = DateTime.Now.Ticks % 2 == 0;
                var withFixedText = DateTime.Now.Ticks % 3 == 0;

                switch (withParams)
                {
                    case true when withFixedText:
                    {
                        var paramValue = _faker.Lorem.Word();

                        return logLevel switch
                        {
                            LogLevel.Trace => () => Log.Verbose("[TRACE] {paramValue} -  This is a trace message with param.", paramValue),
                            LogLevel.Debug => () => Log.Debug("[DEBUG] {paramValue} -  This is a debug message with param.", paramValue),
                            LogLevel.Information => () => Log.Information("[INFORMATION]  {paramValue} - This is a informational message with param.", paramValue),
                            LogLevel.Warning => () => Log.Warning("[WARNING]  {paramValue} - This is a warning message with param.", paramValue),
                            LogLevel.Error => () => Log.Error("[ERROR]  {paramValue} - This is a warning message with param.", paramValue),
                            LogLevel.Critical => () => Log.Fatal("[CRITICAL]  {paramValue} - This is a critical message with param.", paramValue),
                            LogLevel.None => () => { },
                            _ => () => { }
                        };
                    }
                    case true when !withFixedText:
                    {
                        var paramValue = _faker.Lorem.Word();

                        return logLevel switch
                        {
                            LogLevel.Trace => () => Log.Verbose("[TRACE] {paramValue} - " + RandomText, paramValue),
                            LogLevel.Debug => () => Log.Debug("[DEBUG] {paramValue} -  " + RandomText, paramValue),
                            LogLevel.Information => () => Log.Information("[INFORMATION]  {paramValue} - " + RandomText, paramValue),
                            LogLevel.Warning => () => Log.Warning("[WARNING]  {paramValue} - " + RandomText, paramValue),
                            LogLevel.Error => () => Log.Error("[ERROR]  {paramValue} - " + RandomText, paramValue),
                            LogLevel.Critical => () => Log.Fatal("[CRITICAL]  {paramValue} - " + RandomText, paramValue),
                            LogLevel.None => () => { },
                            _ => () => { }
                        };
                    }
                    default:
                        return logLevel switch
                        {
                            LogLevel.Trace => () => Log.Verbose("[TRACE] This is a Trace message."),
                            LogLevel.Debug => () => Log.Debug("[DEBUG] This is a Debug message."),
                            LogLevel.Information => () => Log.Information("[INFORMATION] This is a Information message."),
                            LogLevel.Warning => () => Log.Warning("[WARNING] This is a Warning message."),
                            LogLevel.Error => () => Log.Error("[ERROR] This is a Error message."),
                            LogLevel.Critical => () => Log.Fatal("[CRITICAL] This is a Fatal message."),
                            LogLevel.None => () => { },
                            _ => () => { }
                        };
                }
            }

            return new List<Action>
            {
                GetLog(LogLevel.Trace),
                GetLog(LogLevel.Trace),
                
                GetLog(LogLevel.Debug),
                GetLog(LogLevel.Debug),
                GetLog(LogLevel.Debug),
                GetLog(LogLevel.Debug),
                GetLog(LogLevel.Debug),

                GetLog(LogLevel.Information),
                GetLog(LogLevel.Information),
                GetLog(LogLevel.Information),
                GetLog(LogLevel.Information),
                
                GetLog(LogLevel.Warning),
                GetLog(LogLevel.Warning),
                GetLog(LogLevel.Warning),
                
                GetLog(LogLevel.Error),
                GetLog(LogLevel.Error),
                
                GetLog(LogLevel.Critical)
            };
        }

        var logActions = new List<Action>();

        logActions.AddRange(GetSetOfLogs());
        logActions.AddRange(GetSetOfLogs());
        logActions.AddRange(GetSetOfLogs());


        var random = RandomNumberGenerator.GetInt32(81, 99);
        Console.WriteLine("Random : " + random);
        var take = (int)Math.Ceiling(logActions.Count * (random * 0.01));

        logActions = logActions.OrderBy(_ => Guid.NewGuid())
            .Take(take)
            .ToList();
        
        Console.WriteLine("Starting generation of logs ...");
        Console.WriteLine(Environment.NewLine);

        WriteLogsWithRandomDelay(logActions, withDelay);

        Console.WriteLine(Environment.NewLine);
        Console.WriteLine("Generation of logs completed ...");
    }

    private static void WriteLogsWithRandomDelay(IEnumerable<Action> logActions, bool withDelay)
    {
        foreach (var logAction in logActions)
        {
            logAction();
            
            if (withDelay)
                Thread.Sleep(RandomDelay);
        }
    }
}