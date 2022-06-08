using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using CleanArchitecture.DDD.Application.DTO.Internal;
using FluentValidation;
using Newtonsoft.Json;

namespace CleanArchitecture.DDD.Application.Services;

public class EDCMSyncService : BaseService, IEDCMSyncService
{
    private readonly HttpClient _httpClient;
    private readonly IValidator<ExternalDoctorAddressDTO> _validator;

    private readonly IAsyncPolicy _retryPolicyWithJittering;

    public EDCMSyncService(HttpClient httpClient, IPolicyHolder policyHolder, 
        IValidator<ExternalDoctorAddressDTO> validator, IAppServices appServices)
            : base(appServices)
    {
        _httpClient = httpClient;
        _validator = validator;

        policyHolder.Registry
            .TryGet(PolicyNames.RetryPolicyWithJitter.ToString(), out _retryPolicyWithJittering);
        
    }

    public async Task<IEnumerable<DoctorDTO>> SyncDoctors()
    {
        Log.Information("Syncing doctors");

        var response = await _httpClient.GetAsync("Fake/doctors");
        response.EnsureSuccessStatusCode();

        var doctorDTOList = await response.Content.ReadAsAsync<IReadOnlyList<DoctorDTO>>();
        
        // Prepare to save to database
        // We are using a static method here but AutoMapper could also be used
        ImmutableList<Doctor> doctors = doctorDTOList
            .Select(DoctorDTO.ToDoctor)
            .ToImmutableList();

        await SaveDoctorsInDatabaseAsync(doctors);

        return doctorDTOList;
    }

    /// <summary>
    /// Better Alternative: Implement this as a Azure Serverless function and invoke explicitly from here or
    /// by using a CRON Scheduler
    /// Hangfire cannot serialize EntityFramework
    /// </summary>
    /// <returns></returns>
    public void SyncDoctorsInBackground()
    {
        _ = BackgroundJob.Enqueue(() => SyncDoctorsInBackground(_httpClient, DbContext));
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

    public async Task<IEnumerable<DoctorDTO>> SyncDoctorsWithSomeInvalidData()
    {
        Log.Information("Syncing doctors after input validation ... ");
        
        var response = await _httpClient.GetAsync("Fake/doctors/invalid");
        response.EnsureSuccessStatusCode();

        var parsedResponse = await response.Content.ReadAsAsync<IReadOnlyList<FakeDoctorAddressDTO>>();

        if (parsedResponse.Count == 0)
            return Enumerable.Empty<DoctorDTO>();

        ModelValidationReport<ExternalDoctorAddressDTO> modelValidationReport = GetModelValidationReport(parsedResponse);
        
        if (modelValidationReport.HasInvalidModels)
            NotifyAdminAboutInvalidData(modelValidationReport);

        // This is not necessary here but done only as an example
        // to demonstrate AutoMapper
        var doctorDTOList = AutoMapper
            .Map<IEnumerable<ExternalDoctorAddressDTO>, IEnumerable<DoctorDTO>>(modelValidationReport.ValidModels)
            .ToList();

        // Save valid list in database!
        var doctorList = doctorDTOList
            .Select(DoctorDTO.ToDoctor)
            .ToList();

        await SaveDoctorsInDatabaseAsync(doctorList);

        Log.Information("Sync completed ... ");

        return doctorDTOList;
    }
    

    private ModelValidationReport<ExternalDoctorAddressDTO> GetModelValidationReport(IEnumerable<FakeDoctorAddressDTO> dataFromExternalSystem)
    {
        // Not necessary- It was only to demonstrate use of AutoMapper
        var externalDoctorDTOList = AutoMapper
            .Map<IEnumerable<FakeDoctorAddressDTO>, IEnumerable<ExternalDoctorAddressDTO>>(dataFromExternalSystem)
            .ToList();

        List<GenericModelValidationReport<ExternalDoctorAddressDTO>> errorReport = externalDoctorDTOList
            .Select(doc =>
            {
                var validationResult = _validator.Validate(doc);

                return new GenericModelValidationReport<ExternalDoctorAddressDTO>
                {
                    Model = doc,
                    Valid = validationResult.IsValid,
                    ModelErrors = validationResult.Errors
                        .Select(e => new {e.PropertyName, e.ErrorMessage})
                        .GroupBy(e => e.PropertyName)
                        .Select(e => new ValidationErrorByProperty
                        {
                            PropertyName = e.Key,
                            ErrorMessages = e.Select(err => err.ErrorMessage).ToList()
                        })
                };
            })
            .ToList();

        
        return new ModelValidationReport<ExternalDoctorAddressDTO>(errorReport);
    }

    private void NotifyAdminAboutInvalidData<T>(ModelValidationReport<T> modelValidationReport)
        where T : class, new()
    {
        if (modelValidationReport.HasAllValidModels)
            return;
        
        // TODO: Save as HTML and send as attachment using Weischer Global Email service 
        var validationResult = JsonConvert.SerializeObject(modelValidationReport.Report, Formatting.Indented);
        Log.Warning(validationResult);

        Console.WriteLine();
        Log.Warning("Got {countOfInvalidModels} invalid data from CRM / external system.", modelValidationReport.InvalidModels.Count());
        Console.WriteLine();
    }
    
    private async Task SaveDoctorsInDatabaseAsync(IEnumerable<Doctor> doctors)
    {
        foreach (var doctor in doctors)
        {
            var existingDoctor = await DbContext.Doctors
                // Explicit join using Entity Framework
                .Include(doc => doc.Address)
                .Where(doc => doc.EDCMExternalID == doctor.EDCMExternalID)
                .FirstOrDefaultAsync();

            // New C# syntax (Official recommendation)
            // existingDoctor != null 
            // will fail if '!=' is overloaded
            if (existingDoctor is not null)
            {
                // Not updating names for the sake of simplicity
                
                // Can be optimized when using a Value Object
                if (existingDoctor.Address != doctor.Address)
                {
                    await DbContext.Addresses
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
                // EF will take care that PK-FK values are correctly set
                // Since EF is aware that Doctors and Addresses are linked using a PK-FK relationship
                await DbContext.Doctors.AddAsync(doctor);
            }
        }

        // Wrap Db changes in a Polly Policy
        // Not strictly necessary
        // Only used for demonstration
        await _retryPolicyWithJittering.ExecuteAsync(async () =>
        {
            await DbContext.SaveChangesAsync();
        });
        
    }

}