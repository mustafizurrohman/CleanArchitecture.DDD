using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using Newtonsoft.Json;

namespace CleanArchitecture.DDD.Application.Services;

public class EDCMSyncService : BaseService, IEDCMSyncService
{
    private readonly HttpClient _httpClient;
    private readonly IValidator<ExternalDoctorAddressDTO> _validator;

    private readonly IAsyncPolicy<HttpResponseMessage> _retryPolicy;

    public EDCMSyncService(HttpClient httpClient, IPolicyHolder policyHolder, 
        IValidator<ExternalDoctorAddressDTO> validator, IAppServices appServices)
            : base(appServices)
    {
        _httpClient = httpClient;
        _validator = validator;

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
        ImmutableList<Doctor> doctors = doctorDTOList
            .Select(DoctorDTO.ToDoctor)
            .ToImmutableList();

        await SaveDoctorsInDatabaseAsync(doctors);

        return doctorDTOList;
    }

    /// <summary>
    /// Better Alternative: Implement this as a Azure Serverless function and invoke explicitly from here or
    /// by using a CRON Scheduler
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

    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
    public async Task<IEnumerable<DoctorDTO>> SyncDoctorsWithSomeInvalidData()
    {
        Log.Information("Syncing doctors after input validation ... ");
        
        var response = await _httpClient.GetAsync("Fake/doctors/invalid");
        response.EnsureSuccessStatusCode();

        var parsedResponse = await response.Content.ReadAsAsync<IReadOnlyList<FakeDoctorAddressDTO>>();

        if (parsedResponse.Count == 0)
            return Enumerable.Empty<DoctorDTO>();

        var (validListOfDoctors, invalidListOfDoctors) = ValidateIncomingData(parsedResponse);
        
        if (invalidListOfDoctors.Any())
            NotifyAdminAboutInvalidData(invalidListOfDoctors);

        // This is not necessary here but done only as an example
        // to demonstrate AutoMapper
        var doctorDTOList = AutoMapper
            .Map<IEnumerable<ExternalDoctorAddressDTO>, IEnumerable<DoctorDTO>>(validListOfDoctors)
            .ToList();

        // Save valid list in database!
        var doctorList = doctorDTOList
            .Select(DoctorDTO.ToDoctor)
            .ToList();

        await SaveDoctorsInDatabaseAsync(doctorList);

        Log.Information("Sync completed ... ");

        return doctorDTOList;
    }
    
    private Tuple<IEnumerable<ExternalDoctorAddressDTO>, IEnumerable<ExternalDoctorAddressDTO>> ValidateIncomingData(IEnumerable<FakeDoctorAddressDTO> dataFromExternalSystem)
    {
        // Not necessary- It was only to demonstrate use of AutoMapper
        var externalDoctorDTOList = AutoMapper
            .Map<IEnumerable<FakeDoctorAddressDTO>, IEnumerable<ExternalDoctorAddressDTO>>(dataFromExternalSystem)
            .ToList();

        var errorReport = externalDoctorDTOList
            .Select(doc =>
            {
                var validationResult = _validator.Validate(doc);

                return new
                {
                    Doc = doc,
                    Valid = validationResult.IsValid,
                    ModelErrors = validationResult.Errors
                        .Select(e => new {e.PropertyName, e.ErrorMessage})
                        .GroupBy(e => e.PropertyName)
                        .Select(e => new
                        {
                            Property = e.Key,
                            Errors = e.Select(err => err.ErrorMessage).ToList()
                        })
                };
            })
            .ToList();

        var errorReportAsString = JsonConvert.SerializeObject(errorReport, Formatting.Indented);

        // Log.Warning(errorReportAsString);

        var validListOfDoctors = errorReport
            .Where(re => re.Valid)
            .Select(re => re.Doc);

        var invalidListOfDoctors = errorReport
            .Where(re => !re.Valid)
            .Select(re => re.Doc);
        
        return new Tuple<IEnumerable<ExternalDoctorAddressDTO>, IEnumerable<ExternalDoctorAddressDTO>>
            (validListOfDoctors, invalidListOfDoctors);

    }

    private void NotifyAdminAboutInvalidData(IEnumerable<ExternalDoctorAddressDTO> externalDoctorAddressDTO)
    {
        externalDoctorAddressDTO = externalDoctorAddressDTO.ToList();

        if (!externalDoctorAddressDTO.Any())
            return;

        // Notify admin about invalid data 
        var validationErrors = externalDoctorAddressDTO
            .Select(doc => new
            {
                ID = doc.EDCMExternalID,
                ValidationErrors = _validator.Validate(doc).Errors
                    .GroupBy(err => err.PropertyName)
            })
            .ToList();

        // TODO: Save as HTML and send as attachment using Weischer Global Email service 
        var validationResult = JsonConvert.SerializeObject(validationErrors, Formatting.Indented);
        Log.Warning(validationResult);

        Console.WriteLine();
        Console.WriteLine($"Got {externalDoctorAddressDTO.Count()} invalid data from CRM. Admin must be informed!");
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
                if (existingDoctor.Address.StreetAddress != doctor.Address.StreetAddress
                    || existingDoctor.Address.ZipCode != doctor.Address.ZipCode
                    || existingDoctor.Address.City != doctor.Address.City
                    || existingDoctor.Address.Country != doctor.Address.Country)
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

        await DbContext.SaveChangesAsync();
    }
    
}