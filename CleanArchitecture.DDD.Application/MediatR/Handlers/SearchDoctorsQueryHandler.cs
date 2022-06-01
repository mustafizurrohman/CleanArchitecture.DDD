using CleanArchitecture.DDD.Application.ServicesAggregate;
using CleanArchitecture.DDD.Domain.Exceptions;
using CleanArchitecture.DDD.Domain.ValueObjects;
using Serilog;

namespace CleanArchitecture.DDD.Application.MediatR.Handlers;

public class SearchDoctorsQueryHandler : BaseHandler, IRequestHandler<SearchDoctorsQuery, IEnumerable<DoctorCityDTO>>
{
    public SearchDoctorsQueryHandler(IAppServices appServices)
        : base(appServices)
    {
        
    }

    public async Task<IEnumerable<DoctorCityDTO>> Handle(SearchDoctorsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var firstName = request.FirstName?.Trim() ?? string.Empty;
            var middleName = request.MiddleName?.Trim() ?? string.Empty;
            var lastName  = request.LastName?.Trim() ?? string.Empty;

            var query = DbContext.Doctors.AsQueryable();

            var name = Name.Create(firstName, middleName, lastName);

            if (!string.IsNullOrWhiteSpace(firstName))
                query = query.Where(doc => doc.Name.Firstname.ToLower().Contains(name.Firstname.ToLower()));

            if (!string.IsNullOrWhiteSpace(middleName))
            {

#pragma warning disable CS8602
                // Dereference of a possibly null reference.
                query = query
                    .Where(doc => doc.Name.Middlename != null)
                    .Where(doc => doc.Name.Middlename.ToLower().Contains(name.Middlename.ToLower()));
#pragma warning restore CS8602
                // Dereference of a possibly null reference.
            }

            if (!string.IsNullOrWhiteSpace(lastName))
                query = query.Where(doc => doc.Name.Lastname.ToLower().Contains(name.Lastname.ToLower()));

            var results = await query.AsNoTracking()
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