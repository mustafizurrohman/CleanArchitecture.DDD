namespace CleanArchitecture.DDD.Application.MediatR.Handlers;

public class SyncDoctorCommandHandler : IRequestHandler<SyncDoctorCommand>
{
    private readonly IEDCMSyncService _iedcmSyncService;

    public SyncDoctorCommandHandler(IEDCMSyncService iedcmSyncService)
    {
        _iedcmSyncService = iedcmSyncService;
    }

    public async Task<Unit> Handle(SyncDoctorCommand request, CancellationToken cancellationToken)
    {
        await _iedcmSyncService.SyncDoctors();
        return Unit.Value;
    }
}