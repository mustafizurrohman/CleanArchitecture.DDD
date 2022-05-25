using System.Net;
using AutoMapper;
using CleanArchitecture.DDD.Application.DTO;
using Swashbuckle.AspNetCore.Annotations;

namespace CleanArchitecture.DDD.API.Controllers.Fake;

/// <summary>
/// Fake Controller
/// In a real application this will be another service
/// like CRM
/// </summary>
public class FakeController : BaseAPIController
{

    private static int _attempts = 0;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="autoMapper"></param>
    public FakeController(DomainDbContext dbContext, IMapper autoMapper) 
        : base(dbContext, autoMapper)
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
        Thread.Sleep(2000);

        // Simulate a fake error here
        if (++_attempts % 15 != 0)
            return StatusCode((int)HttpStatusCode.GatewayTimeout);
            
        var faker = new Faker("de");

        var fakeCountries = new List<string>()
        {
            "Germany",
            "Austria",
            "Switzerland"
        };

        var addresses = Enumerable.Range(0, num)
            .Select(_ => new AddressDTO()
            {
                AddressID = Guid.NewGuid(),
                StreetAddress = faker.Address.StreetAddress(),
                ZipCode = faker.Address.ZipCode(),
                City = faker.Address.City(),
                Country = faker.Random.ArrayElement(fakeCountries.ToArray())
            })
            .ToList();

        var names = Enumerable.Range(0, num)
            .Select(_ => Name.Create(faker.Name.FirstName(), faker.Name.LastName()))
            .ToArray();

        var doctors = Enumerable.Range(0, num)
            .Select(_ =>
            {
                var randomName = faker.Random.ArrayElement(names);
                var randomAddress = faker.Random.ArrayElement(addresses.ToArray());

                addresses.Remove(randomAddress);

                return new DoctorDTO
                {
                    Name = randomName,
                    Address = randomAddress
                };
            })
            .ToList();

        return Ok(doctors);
    }



}