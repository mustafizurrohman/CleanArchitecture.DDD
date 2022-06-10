namespace CleanArchitecture.DDD.Application.MediatR.Queries;

public record HashPasswordVerificationQuery(string Password, string HashedPassword)
    : IRequest<bool>;