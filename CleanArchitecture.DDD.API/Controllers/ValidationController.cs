using AutoMapper.QueryableExtensions;
using CleanArchitecture.DDD.Core.ExtensionMethods.FluentValidation.Methods;
using CleanArchitecture.DDD.Core.ExtensionMethods.FluentValidation.Models;

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
            .Where(doc => withModelError ? !doc.GetModelValidationReport().Valid : doc.GetModelValidationReport().Valid)
            .Select(doc => doc.GetModelValidationReport())
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
    public async Task<IActionResult> DemoExtensionMethodForCollection(int num = 100)
    {
        Guard.Against.NegativeOrZero(num);

        var fakeDoctors = _fakeDataService.GetFakeDoctorsWithSomeInvalidData(num).ToList();

        List<FakeDoctorAddressDTO> doctorsToValidate = AutoMapper.Map<IEnumerable<FakeDoctorAddressDTO>>(fakeDoctors).ToList();

        var validationReport = await doctorsToValidate.AsEnumerable().GetModelValidationReportAsync();
        var test = await doctorsToValidate.GetModelValidationReportEnumerableAsync().ToListAsync();

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
