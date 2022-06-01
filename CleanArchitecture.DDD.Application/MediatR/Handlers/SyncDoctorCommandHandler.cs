using CleanArchitecture.DDD.Application.ServicesAggregate;

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
        await _iedcmSyncService.SyncDoctors();
        return Unit.Value;
    }
}