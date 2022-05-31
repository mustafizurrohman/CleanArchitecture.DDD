using System.Collections.Immutable;
using CleanArchitecture.DDD.Application.DTO;
using CleanArchitecture.DDD.Core.Polly;
using CleanArchitecture.DDD.Infrastructure.Persistence.DbContext;
using Hangfire;
using Polly;
using Serilog;

namespace CleanArchitecture.DDD.Application.Services;

public class EDCMSyncService : IEDCMSyncService
{
    private readonly HttpClient _httpClient;
    private readonly DomainDbContext _domainDbContext;

    private readonly IAsyncPolicy<HttpResponseMessage> _retryPolicy;

    public EDCMSyncService(HttpClient httpClient, IPolicyHolder policyHolder, DomainDbContext domainDbContext)
    {
        _httpClient = httpClient;
        _domainDbContext = domainDbContext;
        
        policyHolder.Registry
            .TryGet(HttpPolicyNames.HttpRetryPolicy.ToString(), out _retryPolicy);
        
    }

    public async Task<IEnumerable<DoctorDTO>> SyncDoctors()
    {
        Log.Information("Syncing doctors");

        var response = await _httpClient.GetAsync("Fake/doctors");
        response.EnsureSuccessStatusCode();

        var doctorDTOList = await response.Content.ReadAsAsync<IReadOnlyList<DoctorDTO>>();

        // Prepare to save to database
        // We are using a static method here but AutoMapper could also be used
        var doctors = doctorDTOList
            .Select(DoctorDTO.ToDoctor)
            .ToImmutableList();

        // Entity Framework is aware that Doctors an Address have a PK-FK Relationship
        // So it will take care that the keys are properly created and linked.
        await _domainDbContext.AddRangeAsync(doctors);
        await _domainDbContext.SaveChangesAsync();

        return doctorDTOList;
        
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public void SyncDoctorsInBackground()
    {
        _ = BackgroundJob.Enqueue(() => SyncDoctors());

    }
}