using System.Collections.Immutable;
using CleanArchitecture.DDD.Application.DTO;
using CleanArchitecture.DDD.Core.Polly;
using CleanArchitecture.DDD.Domain.ValueObjects;
using Hangfire;
using Polly;
using Z.EntityFramework.Plus;

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

        // TODO: Validate using FluentValidation the recieved data here and infrom if and which data are invalid 
        // TODO: according to business rules


        // Prepare to save to database
        // We are using a static method here but AutoMapper could also be used
        ImmutableList<Doctor> doctors = doctorDTOList
            .Select(DoctorDTO.ToDoctor)
            .ToImmutableList();
        
        foreach (var doctor in doctors)
        {
            var existingDoctor = await _domainDbContext.Doctors
                .Where(doc => doc.EDCMExternalID == doctor.EDCMExternalID)
                .FirstOrDefaultAsync();

            if (existingDoctor is not null)
            {
                // Not updating names for the sake of simplicity

                if (existingDoctor.Address != doctor.Address)
                {
                    await _domainDbContext.Addresses
                        .Where(addr => addr.AddressID == existingDoctor.AddressId)
                        .UpdateAsync(_ => new Address()
                        {
                            City = doctor.Address.City,
                            Country = doctor.Address.Country,
                            StreetAddress = doctor.Address.StreetAddress,
                            ZipCode = doctor.Address.ZipCode
                        });
                }
            }
            else
            {
                await _domainDbContext.Doctors.AddAsync(doctor);
            }
        }

        await _domainDbContext.SaveChangesAsync();
        return doctorDTOList;
    }

    /// <summary>
    /// Better Alternative: Implement this as a Azure Serverless function and invoke explicitly from here or
    /// by using a CRON Scheduler
    /// </summary>
    /// <returns></returns>
    public void SyncDoctorsInBackground()
    {
        _ = BackgroundJob.Enqueue(() => SyncDoctorsInBackground(_httpClient, _domainDbContext));
    }

    // ReSharper disable once MemberCanBePrivate.Global
    public async Task<IEnumerable<DoctorDTO>> SyncDoctorsInBackground(HttpClient httpClient, DomainDbContext domainDbContext)
    {
        Log.Information("Syncing doctors in background");

        var response = await httpClient.GetAsync("Fake/doctors");
        response.EnsureSuccessStatusCode();

        var doctorDTOList = await response.Content.ReadAsAsync<IReadOnlyList<DoctorDTO>>();

        // Prepare to save to database
        // We are using a static method here but AutoMapper could also be used
        var doctors = doctorDTOList
            .Select(DoctorDTO.ToDoctor)
            .ToImmutableList();

        // Entity Framework is aware that Doctors an Address have a PK-FK Relationship
        // So it will take care that the keys are properly created and linked.
        await domainDbContext.AddRangeAsync(doctors);
        await domainDbContext.SaveChangesAsync();

        return doctorDTOList;
    }
}