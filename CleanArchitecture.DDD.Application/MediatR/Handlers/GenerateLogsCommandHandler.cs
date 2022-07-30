using System.Linq;
using System.Security.Cryptography;

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
            return new List<Action>
            {
                () =>
                {
                    Log.Verbose("[VERBOSE] " + RandomText);
                },
                () =>
                {
                    Log.Debug("[DEBUG] " + RandomText);
                },
                () =>
                {
                    Log.Information("[INFORMATION] " + RandomText);
                },
                () =>
                {
                    Log.Warning("[WARNING] " + RandomText);
                },
                () =>
                {
                    Log.Warning("[WARNING] " + RandomText);
                },
                () =>
                {
                    Log.Warning("[WARNING] " + RandomText);
                },
                () =>
                {
                    Log.Error("[ERROR] " + RandomText);
                },
                () =>
                {
                    Log.Error("[ERROR] " + RandomText);
                },
                () =>
                {
                    Log.Fatal("[CRITICAL] " + RandomText);
                },
                () =>
                {
                    Log.Information("[INFORMATION] " + RandomText);
                },
                () =>
                {
                    Log.Information("[INFORMATION] " + RandomText);
                },
                () =>
                {
                    Log.Information("[INFORMATION] " + RandomText);
                }
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