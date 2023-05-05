namespace CleanArchitecture.DDD.Application.MediatR.Handlers;

public sealed class HashPasswordVerificationQueryHandler 
    : IRequestHandler<HashPasswordVerificationQuery, bool>
{
    private readonly IPasswordService _passwordService;

    public HashPasswordVerificationQueryHandler(IPasswordService passwordService)
    {
        _passwordService = passwordService;
    }


    public Task<bool> Handle(HashPasswordVerificationQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(_passwordService.VerifyPassword(request.Password, request.HashedPassword));
    }
}