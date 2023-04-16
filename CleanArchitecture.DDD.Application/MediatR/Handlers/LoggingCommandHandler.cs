namespace CleanArchitecture.DDD.Application.MediatR.Handlers;

public class LoggingCommandHandler : IRequestHandler<LoggingCommand>
{
    private readonly IEDCMSyncService _edcmSyncService;
    private readonly IMediator _mediator;

    public LoggingCommandHandler(IEDCMSyncService edcmSyncService, IMediator mediator)
    {
        _edcmSyncService = edcmSyncService;
        _mediator = mediator;
    }
    
    public async Task Handle(LoggingCommand request, CancellationToken cancellationToken)
    {
        var generateLogsCommand = new GenerateLogsCommand(1);
        await _mediator.Send(generateLogsCommand, cancellationToken);

        await _edcmSyncService.GetFakeDoctors();

        var now = DateTime.Now;

        //Note: We always throw an exception no matter what
        if (now.Second % 2 == 0)
            throw new NotImplementedException();

        if (now.Second % 3 == 0)
            throw new InsufficientMemoryException();

        if (now.Second % 1 == 0)
            throw new ArrayTypeMismatchException("Don't know what to throw ...");
    }
}