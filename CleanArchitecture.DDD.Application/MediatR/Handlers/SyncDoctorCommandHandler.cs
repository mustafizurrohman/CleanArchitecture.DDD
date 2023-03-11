namespace CleanArchitecture.DDD.Application.MediatR.Handlers;

public class SyncDoctorCommandHandler : BaseHandler, IRequestHandler<SyncDoctorCommand>
{
    private readonly IEDCMSyncService _iedcmSyncService;

    public SyncDoctorCommandHandler(IEDCMSyncService iedcmSyncService, IAppServices appServices)
        : base(appServices)
    {
        _iedcmSyncService = iedcmSyncService;
    }

    public async Task Handle(SyncDoctorCommand request, CancellationToken cancellationToken)
    {
        if (request.SimulateError) {
            await _iedcmSyncService.SyncDoctorsWithSomeInvalidData();
        }
        else {
            await _iedcmSyncService.SyncDoctors();
        }
        
    }
}