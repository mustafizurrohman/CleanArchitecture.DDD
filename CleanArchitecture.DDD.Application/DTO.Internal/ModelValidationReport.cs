namespace CleanArchitecture.DDD.Application.DTO.Internal;

internal class ModelValidationReport : GenericModelValidationReport<ExternalDoctorAddressDTO>
{
    
    public IEnumerable<GenericModelValidationReport<ExternalDoctorAddressDTO>> Report { get; init; }
    
    public IEnumerable<ExternalDoctorAddressDTO> ValidModels =>
        Report.Where(r => r.Valid).Select(r => r.Model).ToList();

    public IEnumerable<ExternalDoctorAddressDTO> InvalidModels =>
        Report.Where(r => !r.Valid).Select(r => r.Model).ToList();

    public bool HasInvalidModels => Report.Any(r => !r.Valid);

    public bool HasAllValidModels => Report.All(r => r.Valid);

    public ModelValidationReport(IEnumerable<GenericModelValidationReport<ExternalDoctorAddressDTO>> report)
    {
        this.Report = report;
    } 

}