using System.Security.Cryptography;

namespace CleanArchitecture.DDD.Application.MediatR.Handlers;

public class HashPasswordQueryHandler : IRequestHandler<HashPasswordQuery, string>
{
    private readonly IPasswordService _passwordService;
    

    public HashPasswordQueryHandler(IPasswordService passwordService)
    {
        _passwordService = passwordService;
    }

    public Task<string> Handle(HashPasswordQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(_passwordService.HashPassword(request.Password));
    }
}