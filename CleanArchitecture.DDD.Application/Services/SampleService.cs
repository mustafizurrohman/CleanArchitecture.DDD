using System.Collections.Immutable;
using CleanArchitecture.DDD.Application.DTO;
using CleanArchitecture.DDD.Core.Polly;
using CleanArchitecture.DDD.Domain.ValueObjects;
using CleanArchitecture.DDD.Infrastructure.Persistence.DbContext;
using Polly;

namespace CleanArchitecture.DDD.Application.Services;

public class SampleService : ISampleService
{
    private readonly HttpClient _httpClient;
    private readonly DomainDbContext _domainDbContext;

    private readonly IAsyncPolicy<HttpResponseMessage> _retryPolicy;

    public SampleService(HttpClient httpClient, IPolicyHolder policyHolder, DomainDbContext domainDbContext)
    {
        _httpClient = httpClient;
        _domainDbContext = domainDbContext;
        
        policyHolder.Registry
            .TryGet(HttpPolicyNames.HttpRetryPolicy.ToString(), out _retryPolicy);
        
    }

    public async Task<IEnumerable<DoctorDTO>>  TestHttpClient()
    {
        var response = await _httpClient.GetAsync("Fake/doctors");
        response.EnsureSuccessStatusCode();

        var doctorDTOList = await response.Content.ReadAsAsync<IReadOnlyList<DoctorDTO>>();

        // Save to database
        var doctors = doctorDTOList
            .Select(doc =>
            {
                // ReSharper disable once IdentifierTypo
                var addrDTO = doc.Address;
                var docAddress = Address.Create(addrDTO.StreetAddress, addrDTO.ZipCode, addrDTO.City, addrDTO.Country);
                
                var docName = Name.Copy(doc.Name, false);

                return Doctor.Create(docName, docAddress);
            })
            .ToImmutableList();

        // Entity Framework is aware that Doctors an Address have a PK-FK Relationship
        // So it will take care that the keys are properly created and linked.
        await _domainDbContext.AddRangeAsync(doctors);
        await _domainDbContext.SaveChangesAsync();

        return doctorDTOList;
        
    }
}