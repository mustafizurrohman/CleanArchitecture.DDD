namespace CleanArchitecture.DDD.API.Controllers;

public class PasswordController : BaseAPIController
{
    public PasswordController(IAppServices appServices) : base(appServices)
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="password"></param>
    /// <returns></returns>
    [ApiExplorerSettings(IgnoreApi = false)]
    [HttpGet("hash", Name = "Hash Password")]
    [SwaggerOperation(
        Summary = "Gets doc from a fake external data service",
        Description = "No or default authentication required",
        OperationId = "Hash Password",
        Tags = new[] { "Password" }
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> HashPassword(string password)
    {
        var hashPasswordQuery = new HashPasswordQuery(password);
        var result = await Mediator.Send(hashPasswordQuery);

        return Ok(result);
    }
}