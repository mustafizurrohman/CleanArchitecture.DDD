namespace CleanArchitecture.DDD.Application.MediatR.Queries.HashPassword;

public record HashPasswordQuery(string Password)
    : IRequest<string>, IRequest<bool>;