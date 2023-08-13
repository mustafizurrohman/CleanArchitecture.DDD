using CleanArchitecture.DDD.Application.MediatR.Queries.HashPassword;
using CleanArchitecture.DDD.Application.MediatR.Queries.HashPasswordVerification;

namespace CleanArchitecture.DDD.API.Controllers;

public class PasswordController : BaseAPIController
{
    private const string DefaultControllerTag = "Password";

    public PasswordController(IAppServices appServices) : base(appServices)
    {
    }

    /// <summary>
    /// Hashes a password
    /// </summary>
    /// <param name="password"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [ApiExplorerSettings(IgnoreApi = false)]
    [HttpGet("hash", Name = "Hash Password")]
    [SwaggerOperation(
        Summary = "Hash a password",
        Description = DefaultDescription,
        OperationId = "Hash Password",
        Tags = new[] { DefaultControllerTag }
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> HashPassword(string password, CancellationToken cancellationToken)
    {
        var hashPasswordQuery = new HashPasswordQuery(password);
        var result = await Mediator.Send(hashPasswordQuery, cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Verifies a hashed password
    /// </summary>
    /// <param name="password">Password</param>
    /// <param name="hashedPassword">Hashed password</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [ApiExplorerSettings(IgnoreApi = false)]
    [HttpGet("hash/verify", Name = "Verify Hash Password")]
    [SwaggerOperation(
        Summary = "Verify a password with provided hash value",
        Description = DefaultDescription,
        OperationId = "Verify Hashed Password",
        Tags = new[] { DefaultControllerTag }
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> VerifyHashPassword(string password, string hashedPassword, CancellationToken cancellationToken)
    {
        var hashPasswordVerificationQuery = new HashPasswordVerificationQuery(password, hashedPassword);
        var result = await Mediator.Send(hashPasswordVerificationQuery, cancellationToken);

        return Ok(result);
    }

}