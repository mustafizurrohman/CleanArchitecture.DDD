namespace CleanArchitecture.DDD.Application.MediatR.Queries.HashPasswordVerification;

public sealed class HashPasswordVerificationQueryHandler(IPasswordService passwordService) 
    : IRequestHandler<HashPasswordVerificationQuery, bool>
{
    public Task<bool> Handle(HashPasswordVerificationQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(passwordService.VerifyPassword(request.Password, request.HashedPassword));
    }
}