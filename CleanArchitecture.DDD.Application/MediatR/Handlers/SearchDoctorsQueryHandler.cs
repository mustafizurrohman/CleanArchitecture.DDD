namespace CleanArchitecture.DDD.Application.MediatR.Handlers;

public class SearchDoctorsQueryHandler : BaseHandler, IRequestHandler<SearchDoctorsQuery, IEnumerable<DoctorCityDTO>>
{
    private readonly IValidator<Name> _nameValidator;

    public SearchDoctorsQueryHandler(IAppServices appServices, IValidator<Name> nameValidator)
        : base(appServices)
    {
        _nameValidator = nameValidator;
    }

    public async Task<IEnumerable<DoctorCityDTO>> Handle(SearchDoctorsQuery request, CancellationToken cancellationToken)
    {
        // Ignoring spaces at first and last of input
        var firstName = request.FirstName?.Trim() ?? string.Empty;
        var lastName  = request.LastName?.Trim() ?? string.Empty;
        
        // Create a name without validation
        var name = Name.Create(firstName, string.Empty, lastName, false);
        
        if (request.And)
        {
            var validationResult = await _nameValidator.ValidateAsync(name, cancellationToken);

            // If we are searching all fields and the input does not represent a valid
            // domain model we do not need to search the database at all because 
            // it cannot be present in the database
            if (!validationResult.IsValid)
                return Enumerable.Empty<DoctorCityDTO>();
        }

        var results = await DbContext.Doctors.AsNoTracking()
            .SearchByName(name, request.And)
            .ProjectTo<DoctorCityDTO>(AutoMapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return results;
    }

}