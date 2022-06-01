using CleanArchitecture.DDD.Application.ExtensionMethods;
using CleanArchitecture.DDD.Domain.Exceptions;
using CleanArchitecture.DDD.Domain.ValueObjects;
using FluentValidation;

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
        try
        {
            var firstName = request.FirstName?.Trim() ?? string.Empty;
            var lastName  = request.LastName?.Trim() ?? string.Empty;

            
            Name name = Name.Create(firstName, string.Empty, lastName, request.And);

            if (request.And)
            {
                var validationResult = await _nameValidator.ValidateAsync(name, cancellationToken);

                if (!validationResult.IsValid)
                    return Enumerable.Empty<DoctorCityDTO>();
            }

            var results = await DbContext.Doctors.AsNoTracking()
                .SearchByName(name, request.And)
                .ProjectTo<DoctorCityDTO>(AutoMapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return results;
        }
        catch (DomainValidationException ex)
        {
            Log.Information(ex, "Smart optimization");

            // If an Exception of this type of thrown it means that the 
            // search query does not represent a valid domain model and in that case we do not need
            // to search the database at all
            return Enumerable.Empty<DoctorCityDTO>();
        }
    }

}