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
        Summary = "Hash a password",
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

    /// <summary>
    /// TODO: Finish this
    /// </summary>
    /// <param name="password">Password</param>
    /// <param name="hashedPassword">Hashed password</param>
    /// <returns></returns>
    [ApiExplorerSettings(IgnoreApi = false)]
    [HttpGet("hash/verify", Name = "Verify Hash Password")]
    [SwaggerOperation(
        Summary = "Verify a password with provided hash value",
        Description = "No or default authentication required",
        OperationId = "Verify Hashed Password",
        Tags = new[] { "Password" }
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> VerifyHashPassword(string password, string hashedPassword)
    {
        var hashPasswordVerificationQuery = new HashPasswordVerificationQuery(password, hashedPassword);
        var result = await Mediator.Send(hashPasswordVerificationQuery);

        return Ok(result);
    }

}