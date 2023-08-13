namespace CleanArchitecture.DDD.Application.MediatR.Queries.HashPasswordVerification;

public record HashPasswordVerificationQuery(string Password, string HashedPassword)
    : IRequest<bool>;