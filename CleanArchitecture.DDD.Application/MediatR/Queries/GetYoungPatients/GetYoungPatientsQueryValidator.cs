namespace CleanArchitecture.DDD.Application.MediatR.Queries.GetYoungPatients;

public class GetYoungPatientsQueryValidator : AbstractValidator<GetYoungPatientsQuery>
{
    public GetYoungPatientsQueryValidator()
    {
        RuleFor(prop => prop.Years)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Number of years must be positive.");
    }
}

