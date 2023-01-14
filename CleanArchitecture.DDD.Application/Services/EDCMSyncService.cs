using System.Collections.Immutable;

namespace CleanArchitecture.DDD.Application.Services;

public class EDCMSyncService : BaseService, IEDCMSyncService
{
    private readonly HttpClient _httpClient;
    private readonly IValidator<FakeDoctorAddressDTO> _validator;
    private readonly INotificationService _notificationService;

    private readonly IAsyncPolicy _retryPolicyWithJittering;

    public EDCMSyncService(HttpClient httpClient, IPolicyHolder policyHolder, 
        IValidator<FakeDoctorAddressDTO> validator, IAppServices appServices,
        INotificationService notificationService)
            : base(appServices)
    {
        _httpClient = httpClient;
        _validator = validator;
        _notificationService = notificationService;

        policyHolder.Registry
            .TryGet(PolicyNames.RetryPolicyWithJitter.ToString(), out _retryPolicyWithJittering);
        
    }

    public async Task<IEnumerable<Doctor>> GetFakeDoctors()
    {
        var response = await _httpClient.GetAsync("Fake/doctors");
        response.EnsureSuccessStatusCode();

        var parsedResponse = await response.Content.ReadAsAsync<IReadOnlyList<FakeDoctorAddressDTO>>();

        var doctorDTOList = AutoMapper.Map<List<DoctorDTO>>(parsedResponse);

        var doctors = doctorDTOList
            .Select(DoctorDTO.ToDoctor)
            .ToImmutableList();

        return doctors;
    }

    public async Task<IEnumerable<DoctorDTO>> SyncDoctors()
    {
        Log.Information("Syncing doctors");

        var response = await _httpClient.GetAsync("Fake/doctors");
        response.EnsureSuccessStatusCode();

        var parsedResponse = await response.Content.ReadAsAsync<IReadOnlyList<FakeDoctorAddressDTO>>();

        var doctorDTOList = AutoMapper.Map<List<DoctorDTO>>(parsedResponse);

        // Prepare to save to database
        // We are using a static method here but AutoMapper could also be used
        var doctors = doctorDTOList
            .Select(DoctorDTO.ToDoctor)
            .ToImmutableList();
        
        // Entity Framework is aware that Doctors an Address have a PK-FK Relationship
        // So it will take care that the keys are properly created and linked.
        await DbContext.AddRangeAsync(doctors);
        await DbContext.SaveChangesAsync();

        return doctorDTOList;
    }

    #region -- BackGround --

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

        var parsedResponse = await response.Content.ReadAsAsync<IReadOnlyList<FakeDoctorAddressDTO>>();

        var doctorDTOList = AutoMapper.Map<List<DoctorDTO>>(parsedResponse);

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

    #endregion
    
    public async Task<IEnumerable<DoctorDTO>> SyncDoctorsWithSomeInvalidData()
    {
        Log.Information("Syncing doctors after input validation ... ");
        
        var response = await _httpClient.GetAsync("Fake/doctors/invalid");
        response.EnsureSuccessStatusCode();

        var parsedResponse = await response.Content.ReadAsAsync<IReadOnlyList<FakeDoctorAddressDTO>>();

        if (parsedResponse.Count == 0)
            return Enumerable.Empty<DoctorDTO>();

        var modelCollectionValidationReport = await parsedResponse.GetModelValidationReportAsync(_validator);

        if (modelCollectionValidationReport.HasInvalidModels)
            await _notificationService.NotifyAdminAboutInvalidData(modelCollectionValidationReport);

        // This is not necessary here but done only as an example to demonstrate AutoMapper
        var doctorDTOList = AutoMapper
            .Map<IEnumerable<DoctorDTO>>(modelCollectionValidationReport.ValidModels)
            .ToList();
        
        // Save valid list in database!
        var doctorList = doctorDTOList
            .Select(DoctorDTO.ToDoctor);

        await SaveDoctorsInDatabaseAsync(doctorList);

        Log.Information("Sync completed ... ");

        return doctorDTOList;
    }
    
    private async Task SaveDoctorsInDatabaseAsync(IEnumerable<Doctor> doctors)
    {
        foreach (var doctor in doctors)
        {
            var existingDoctor = await DbContext.Doctors
                // Explicit join using Entity Framework
                .Include(doc => doc.Address)
                .FirstOrDefaultAsync(doc => doc.EDCMExternalID == doctor.EDCMExternalID);

            // New C# syntax (Official recommendation)
            // existingDoctor != null 
            // will fail if '!=' is overloaded
            if (existingDoctor is not null)
            {
                // Not updating names for the sake of simplicity
                
                if (existingDoctor.Address == doctor.Address) {
                    Helper.LogWithSpace(() => Log.Information("Not updating Address with ID {addressID} because it is unchanged.", existingDoctor.Address.AddressID));
                    continue;
                }

                #region -- EF Extensions (To be fixed) --

                // TODO: Investigate why this throws a null reference exception
                //await DbContext.Addresses
                //    .Where(addr => addr.AddressID == existingDoctor.AddressId)
                //    .UpdateAsync(_ => new Address()
                //    {
                //        City = doctor.Address.City,
                //        Country = doctor.Address.Country,
                //        StreetAddress = doctor.Address.StreetAddress,
                //        ZipCode = doctor.Address.ZipCode
                //    });

                #endregion

                var address = await DbContext.Addresses
                    .SingleAsync(addr => addr.AddressID == existingDoctor.AddressId);
                
                address.UpdateAddress(doctor.Address);

                Helper.LogWithSpace(() => Log.Information("Updating Address with ID {addressID}", existingDoctor.Address.AddressID));
            }
            else
            {
                // EF will take care that PK-FK values are correctly set
                // Since EF is aware that Doctors and Addresses are linked using a PK-FK relationship
                await DbContext.AddAsync(doctor);

                Helper.LogWithSpace(() => Log.Information("Inserting Address with ID {addressID}", doctor.Address.AddressID));
            }
        }

        // Wrap Db changes in a Polly Policy
        // Not strictly necessary- Only used for demonstration
        await _retryPolicyWithJittering.ExecuteAsync(async () =>
        {
            await DbContext.SaveChangesAsync();
        });
        
    }

    

}