namespace CleanArchitecture.DDD.Application.MediatR.Queries;

public record GetYoungPatientsQuery(int Years, bool IncludeSoftDeleted)
    : IRequest<IEnumerable<PatientMasterDataDTO>>;


