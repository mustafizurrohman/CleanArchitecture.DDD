using System.Linq;
using System.Runtime.InteropServices.ComTypes;
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
            GenerateLogs(request.WithDelay);

            if (request.WithDelay)
                Thread.Sleep((i+1) * RandomDelay);
            
        }

        return Unit.Value;
    }

    private void GenerateLogs(bool withDelay)
    {
        IEnumerable<Action> GetSetOfLogs()
        {
            Action GetLog(LogLevel logLevel)
            {
                var withParams = DateTime.Now.Ticks % 2 == 0;
                
                if (withParams)
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
        
        WriteLogsWithRandomDelay(logActions, withDelay);
    }

    private void WriteLogsWithRandomDelay(IEnumerable<Action> logActions, bool withDelay)
    {
        foreach (var logAction in logActions)
        {
            logAction();
            
            if (withDelay)
                Thread.Sleep(RandomDelay);
        }
    }
}