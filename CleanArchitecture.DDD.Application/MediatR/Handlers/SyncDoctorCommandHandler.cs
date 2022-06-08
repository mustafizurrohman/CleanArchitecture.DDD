namespace CleanArchitecture.DDD.Application.MediatR.Handlers;

public class SyncDoctorCommandHandler : BaseHandler, IRequestHandler<SyncDoctorCommand>
{
    private readonly IEDCMSyncService _iedcmSyncService;

    public SyncDoctorCommandHandler(IEDCMSyncService iedcmSyncService, IAppServices appServices)
        : base(appServices)
    {
        _iedcmSyncService = iedcmSyncService;
    }

    public async Task<Unit> Handle(SyncDoctorCommand request, CancellationToken cancellationToken)
    {
        if (request.SimulateError)
            await _iedcmSyncService.SyncDoctorsWithSomeInvalidData();
        else
        {
            throw new NotImplementedException();
            // await _iedcmSyncService.SyncDoctors();
        }

        return Unit.Value;
    }
}