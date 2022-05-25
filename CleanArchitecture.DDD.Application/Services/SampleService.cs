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
            .TryGet<IAsyncPolicy<HttpResponseMessage>>(HttpPolicyNames.HttpRetryPolicy.ToString(), out _retryPolicy);
        
    }

    public async Task<IEnumerable<DoctorDTO>> TestHttpClient()
    {
        var response = await _httpClient.GetAsync("Fake/doctors");

        response.EnsureSuccessStatusCode();

        var doctorDTOList = await response.Content.ReadAsAsync<IReadOnlyList<DoctorDTO>>();

        // Save to database
        var doctors = doctorDTOList
            .Select(doc => 
            {
                var docAddress = Address.Create(doc.Address.StreetAddress, doc.Address.ZipCode, doc.Address.City, doc.Address.Country);
                var docName = Name.Copy(doc.Name, false);

                return Doctor.Create(docName, docAddress);
            })
            .ToImmutableList();

        await _domainDbContext.Doctors.AddRangeAsync(doctors);
        await _domainDbContext.SaveChangesAsync();

        return doctorDTOList;
        
    }
}