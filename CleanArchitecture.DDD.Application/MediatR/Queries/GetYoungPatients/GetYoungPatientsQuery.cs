namespace CleanArchitecture.DDD.Application.MediatR.Queries.GetYoungPatients;

public record GetYoungPatientsQuery(int Years, bool IncludeSoftDeleted)
    : IRequest<IEnumerable<PatientMasterDataDTO>>;


