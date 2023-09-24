namespace CleanArchitecture.DDD.Application.MediatR.Queries.HashPassword;

public sealed class HashPasswordQueryHandler(IPasswordService passwordService) 
    : IRequestHandler<HashPasswordQuery, string>
{
    public Task<string> Handle(HashPasswordQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(passwordService.HashPassword(request.Password));
    }
}