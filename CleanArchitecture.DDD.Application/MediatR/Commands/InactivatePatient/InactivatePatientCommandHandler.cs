using CleanArchitecture.DDD.Application.MediatR.Handlers;

namespace CleanArchitecture.DDD.Application.MediatR.Commands.InactivatePatient;

public sealed class InactivatePatientCommandHandler(IAppServices appServices) 
    : BaseHandler(appServices), IRequestHandler<InactivatePatientCommand>
{
    public async Task Handle(InactivatePatientCommand request, CancellationToken cancellationToken)
    {
        // TODO: Why does this not work?
        // Using ExecuteUpdateAsync does not work on JSON Columns yet
        // await DbContext.Patients
        //   .Where(patient => patient.PatientID == request.ID)
        //   .ExecuteUpdateAsync(patient =>
        //       patient.SetProperty(currentPatient => currentPatient.MasterData.Active, p => false)
        //       , cancellationToken);

        var patient = await DbContext.Patients
            .Include(p => p.MasterData)
            .Where(p => p.MasterData.Active)
            .FirstOrDefaultAsync(p => p.PatientID == request.ID, cancellationToken);

        if (patient is not null)
        {
            patient.MasterData.Active = false;
            await DbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
