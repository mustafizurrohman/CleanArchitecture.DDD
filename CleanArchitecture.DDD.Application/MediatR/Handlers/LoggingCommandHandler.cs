using System.Security.Cryptography;

namespace CleanArchitecture.DDD.Application.MediatR.Handlers;

public class LoggingCommandHandler : IRequestHandler<LoggingCommand>
{
    private readonly IEDCMSyncService _edcmSyncService;

    public LoggingCommandHandler(IEDCMSyncService edcmSyncService)
    {
        _edcmSyncService = edcmSyncService;
    }

    private int RandomDelay => RandomNumberGenerator.GetInt32(750, 1000);

    public async Task<Unit> Handle(LoggingCommand request, CancellationToken cancellationToken)
    {
        var faker = new Faker();

        Thread.Sleep(RandomDelay);
        Log.Information("Testing if logs are co-related. {param} is a random parameter", faker.Lorem.Word());

        Thread.Sleep(RandomDelay);
        Log.Verbose("A verbose log message");

        Thread.Sleep(RandomDelay);
        Log.Debug("Here is a debug message with param {message}", "debug message param");

        Thread.Sleep(RandomDelay);
        Log.Fatal("This should actually never happen {param}", faker.Lorem.Sentence());

        Thread.Sleep(RandomDelay);
        Log.Error("Logging before throwing an exception .... ");

        Thread.Sleep(RandomDelay);
        
        await _edcmSyncService.GetFakeDoctors();

        var now = DateTime.Now;

        if (now.Second % 2 == 0)
            throw new NotImplementedException();

        if (now.Second % 3 == 0)
            throw new InsufficientMemoryException();

        throw new Exception("Don't know what to throw ...");

        // ReSharper disable once HeuristicUnreachableCode
        return Unit.Value;
    }
}