using System.Net;
using CleanArchitecture.DDD.Application.DTO;
using CleanArchitecture.DDD.Application.ServicesAggregate;
using Swashbuckle.AspNetCore.Annotations;

namespace CleanArchitecture.DDD.API.Controllers.Fake;

/// <summary>
/// Fake Controller
/// In a real application this will be another service
/// like CRM
/// </summary>
[ApiExplorerSettings(IgnoreApi = true)]
public class FakeController : BaseAPIController
{

    private static int _attempts = 0;

    private static IEnumerable<DoctorDTO> _cachedDoctors = new List<DoctorDTO>();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="appServices"></param>
    public FakeController(IAppServices appServices) 
        : base(appServices)
    {
    }

    [HttpGet("doctors")]
    [SwaggerOperation(
        Summary = "Retrieves all doctors from database",
        Description = "No authentication required",
        OperationId = "GetAllDoctors",
        Tags = new[] { "FakeData" }
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Doctor was retrieved", typeof(IEnumerable<Doctor>))]
    public IActionResult GetFakeDoctors(int num = 10, CancellationToken cancellationToken = default)
    {
        // Simulate a fake delay here
        // Thread.Sleep(2000);

        // Simulate a fake error here
        if (++_attempts % 2 != 0)
            return StatusCode((int)HttpStatusCode.GatewayTimeout);


        if (_cachedDoctors.Any())
        {
            var modifiedDoctors = new List<DoctorDTO>();

            foreach (var cachedDoctor in _cachedDoctors)
            {
                var modifiedDoctor = new DoctorDTO()
                {
                    // Update Address here 
                    Address = new AddressDTO()
                    {
                        AddressID = cachedDoctor.Address.AddressID,
                        StreetAddress = cachedDoctor.Address.StreetAddress + " 2",
                        ZipCode = "22087",
                        City = "Hamburg",
                        Country = cachedDoctor.Address.Country
                    },
                    Name = cachedDoctor.Name
                };
                
                modifiedDoctors.Add(modifiedDoctor);

            }

            return Ok(modifiedDoctors);
        }



        var faker = new Faker("de");

        var fakeCountries = new List<string>()
        {
            "Germany",
            "Austria",
            "Switzerland"
        };

        var fakeAddresses = Enumerable.Range(0, num)
            .Select(_ => new AddressDTO()
            {
                AddressID = Guid.NewGuid(),
                StreetAddress = faker.Address.StreetAddress(),
                ZipCode = faker.Address.ZipCode(),
                City = faker.Address.City(),
                Country = faker.Random.ArrayElement(fakeCountries.ToArray())
            })
            .ToList();

        var fakeNames = Enumerable.Range(0, num)
            .Select(_ => Name.Create(faker.Name.FirstName(), faker.Name.LastName()))
            .ToArray();

        var doctors = Enumerable.Range(0, num)
            .Select(_ =>
            {
                var randomName = faker.Random.ArrayElement(fakeNames);
                var randomAddress = faker.Random.ArrayElement(fakeAddresses.ToArray());

                fakeAddresses.Remove(randomAddress);

                return new DoctorDTO
                {
                    Name = randomName,
                    Address = randomAddress
                };
            })
            .ToList();

        _cachedDoctors = doctors;

        return Ok(doctors);
    }



}