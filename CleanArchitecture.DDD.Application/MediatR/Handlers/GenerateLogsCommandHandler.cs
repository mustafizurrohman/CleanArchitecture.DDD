using System.Linq;
using System.Security.Cryptography;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.DDD.Application.MediatR.Handlers;

public class GenerateLogsCommandHandler : BaseHandler, IRequestHandler<GenerateLogsCommand>
{
    private readonly Faker _faker;

    private int RandomDelay => RandomNumberGenerator.GetInt32(500, 1000);
    private string RandomText => _faker.Lorem.Lines(1);

    public GenerateLogsCommandHandler(IAppServices appServices)
        : base(appServices)
    {
        _faker = new Faker();
    }


    public async Task<Unit> Handle(GenerateLogsCommand request, CancellationToken cancellationToken)
    {
        for (var i = 0; i < request.Iterations; i++)
        {
            GenerateLogs();

            if (request.WithDelay)
                Thread.Sleep((i+1) * RandomDelay);
            
        }

        return Unit.Value;
    }

    private void GenerateLogs()
    {
        IEnumerable<Action> GetSetOfLogs()
        {
            Action GetLog(LogLevel logLevel)
            {
                return logLevel switch
                {
                    LogLevel.Trace => () => Log.Verbose("[TRACE] " + RandomText),
                    LogLevel.Debug => () => Log.Debug("[DEBUG] " + RandomText),
                    LogLevel.Information => () => Log.Information("[INFORMATION] " + RandomText),
                    LogLevel.Warning => () => Log.Warning("[WARNING] " + RandomText),
                    LogLevel.Error => () => Log.Error("[ERROR] " + RandomText),
                    LogLevel.Critical => () => Log.Fatal("[CRITICAL] " + RandomText),
                    LogLevel.None => () => { },
                    _ => () => { }
                };
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

        var take = (int)Math.Ceiling(logActions.Count * 0.75);

        logActions = logActions.OrderBy(_ => Guid.NewGuid())
            .Take(take)
            .ToList();
        
        WriteLogsWithRandomDelay(logActions);
    }

    private void WriteLogsWithRandomDelay(IEnumerable<Action> logActions)
    {
        foreach (var logAction in logActions)
        {
            logAction();
            Thread.Sleep(RandomDelay);
        }
    }
}