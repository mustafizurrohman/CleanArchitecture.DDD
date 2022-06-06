namespace CleanArchitecture.DDD.Application.DTO.Internal;

internal class ModelValidationReport
{
    public IEnumerable<ExternalDoctorAddressDTOModelValidationErrorReport> Report { get; init; }
    
    public IEnumerable<ExternalDoctorAddressDTO> ValidModels =>
        Report.Where(r => r.Valid).Select(r => r.Doctor).ToList();

    public IEnumerable<ExternalDoctorAddressDTO> InvalidModels =>
        Report.Where(r => !r.Valid).Select(r => r.Doctor).ToList();

    public bool HasInvalidModels => Report.Any(r => !r.Valid);

    public bool HasAllValidModels => Report.All(r => r.Valid);

    public ModelValidationReport(IEnumerable<ExternalDoctorAddressDTOModelValidationErrorReport> report)
    {
        this.Report = report;
    }

}