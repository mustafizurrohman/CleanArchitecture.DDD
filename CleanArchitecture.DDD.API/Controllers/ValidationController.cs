using System.Collections.Immutable;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.DDD.Core.ExtensionMethods.FluentValidation.Methods;
using CleanArchitecture.DDD.Core.ExtensionMethods.FluentValidation.Models;
// ReSharper disable PossibleMultipleEnumeration

namespace CleanArchitecture.DDD.API.Controllers;

public class ValidationController : BaseAPIController
{
    private const string DefaultControllerTag = "Validation";

    private readonly IFakeDataService _fakeDataService;

    public ValidationController(IAppServices appServices, IFakeDataService fakeDataService) 
        : base(appServices)
    {
        _fakeDataService = fakeDataService;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    [ApiExplorerSettings(IgnoreApi = false)]
    [HttpPost("ValueObject", Name = "valueObjectValidation")]
    [SwaggerOperation(
        Summary = "Demo of validation of Value Object",
        Description = DefaultDescription,
        OperationId = "Validate Value Object",
        Tags = new[] { DefaultControllerTag }
    )]
    public IActionResult TestNameValueObject(string name)
    {
        var createdName = NameValueObject.Create(name);
        
        if (createdName.IsFailure)
            return BadRequest(createdName.Error.Message);
        
        return Ok(createdName.Value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    [ApiExplorerSettings(IgnoreApi = false)]
    [HttpPost("ValueObject/fluentValidationPipeline")]
    [SwaggerOperation(
        Summary = "Demo of input validation using FluentValidation",
        Description = DefaultDescription,
        OperationId = "Input Validation",
        Tags = new[] { DefaultControllerTag }
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult CreateName([FromBody] Name name)
    {
        return Ok();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="withModelError"></param>
    /// <param name="num"></param>
    /// <returns></returns>
    [ApiExplorerSettings(IgnoreApi = false)]
    [HttpGet("demo/extensionMethod")]
    [SwaggerOperation(
        Summary = "Demo of Extension method for a single object",
        Description = DefaultDescription,
        OperationId = "Extension Method Validation",
        Tags = new[] { DefaultControllerTag }
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult DemoExtensionMethod(bool withModelError = false, int num = 100)
    {
        Guard.Against.NegativeOrZero(num);

        var fakeDoctors = _fakeDataService.GetFakeDoctorsWithSomeInvalidData(num)
            .AsQueryable()
            .ProjectTo<FakeDoctorAddressDTO>(AutoMapper.ConfigurationProvider);
        
        var validationReport = fakeDoctors
            .Where(doc => withModelError ? !doc.GetModelValidationReport(true).Valid : doc.GetModelValidationReport(true).Valid)
            .Select(doc => doc.GetModelValidationReport(true))
            .FirstOrDefault();
        
        return Ok(validationReport);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    [ApiExplorerSettings(IgnoreApi = false)]
    [HttpGet("demo/extensionMethod/collection")]
    [SwaggerOperation(
        Summary = "Demo of Extension method for IEnumerable",
        Description = DefaultDescription,
        OperationId = "Extension Method IEnumerable Validation",
        Tags = new[] { DefaultControllerTag }
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DemoExtensionMethodForCollection(int num = 100, CancellationToken cancellationToken = default)
    {
        Guard.Against.NegativeOrZero(num);

        var fakeDoctors = _fakeDataService.GetFakeDoctorsWithSomeInvalidData(num).ToList();

        var doctorsToValidate = AutoMapper.Map<IEnumerable<FakeDoctorAddressDTO>>(fakeDoctors);

        var validationReport = await doctorsToValidate.GetModelValidationReportAsync();

        _ = doctorsToValidate.GetModelValidationReport();
        _ = doctorsToValidate.ToList().GetModelValidationReport();
        _ = doctorsToValidate.ToArray().GetModelValidationReport();
        _ = doctorsToValidate.ToImmutableArray().GetModelValidationReport();
        _ = doctorsToValidate.ToImmutableList().GetModelValidationReport();

        _ = await doctorsToValidate.GetModelValidationReportAsync();
        _ = await doctorsToValidate.ToList().GetModelValidationReportAsync();
        _ = await doctorsToValidate.ToImmutableList().GetModelValidationReportAsync();
        _ = await doctorsToValidate.ToArray().GetModelValidationReportAsync();
        _ = await doctorsToValidate.ToImmutableArray().GetModelValidationReportAsync();
        
        _ = await doctorsToValidate.GetModelValidationReportEnumerableAsync().ToListAsync(cancellationToken);
        _ = await doctorsToValidate.GetModelValidationReportEnumerableAsync().ToArrayAsync(cancellationToken);
        _ = await doctorsToValidate.ToArray().GetModelValidationReportEnumerableAsync().ToListAsync(cancellationToken);
        _ = await doctorsToValidate.ToImmutableArray().GetModelValidationReportEnumerableAsync().ToListAsync(cancellationToken);
        _ = await doctorsToValidate.ToList().GetModelValidationReportEnumerableAsync().ToListAsync(cancellationToken);
        _ = await doctorsToValidate.ToImmutableList().GetModelValidationReportEnumerableAsync().ToListAsync(cancellationToken);

        var validationReportJson = validationReport.ToFormattedJsonFailSafe();
        Console.Write(validationReportJson);

        return Ok(validationReport);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    [ApiExplorerSettings(IgnoreApi = false)]
    [HttpGet("demo/extensionMethod/error/")]
    [SwaggerOperation(
        Summary = "Demo of incorrect usage of Extension method",
        Description = DefaultDescription,
        OperationId = "Extension Method Validation Incorrect usage",
        Tags = new[] { DefaultControllerTag }
    )]
    public async Task<IActionResult> DemoExtensionMethodErrorForObject(int num = 100)
    {
        Guard.Against.NegativeOrZero(num);

        var fakeDoctors = _fakeDataService.GetFakeDoctorsWithSomeInvalidData(num).ToList();

        var doctorToValidate = fakeDoctors.First();

        // We have not defined a validator for ExternalFakeDoctorAddressDTO
        // So this  will throw an exception at runtime
        var validationReport = await doctorToValidate.GetModelValidationReportAsync();
        
        return Ok(validationReport);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    [ApiExplorerSettings(IgnoreApi = false)]
    [HttpGet("demo/extensionMethod/error/collection")]
    [SwaggerOperation(
        Summary = "Demo of incorrect usage of Extension method for IEnumerable",
        Description = DefaultDescription,
        OperationId = "Extension Method IEnumerable Incorrect usage",
        Tags = new[] { DefaultControllerTag }
    )]
    public async Task<IActionResult> DemoExtensionMethodErrorForCollection(int num = 100)
    {
        Guard.Against.NegativeOrZero(num);

        var fakeDoctors = _fakeDataService.GetFakeDoctorsWithSomeInvalidData(num).ToList();
        
        // We have not defined a validator for ExternalFakeDoctorAddressDTO
        // So this  will throw an exception at runtime
        var validationReport = await fakeDoctors.GetModelValidationReportAsync();
        
        return Ok(validationReport);
    }

}
