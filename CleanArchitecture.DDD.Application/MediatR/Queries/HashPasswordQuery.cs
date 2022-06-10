namespace CleanArchitecture.DDD.Application.MediatR.Queries;

public record HashPasswordQuery(string Password)   
    : IRequest<string>;