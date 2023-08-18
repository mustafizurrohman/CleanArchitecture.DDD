namespace CleanArchitecture.DDD.Application.MediatR.PipelineBehaviours;

public class TransactionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly DomainDbContext _domainDbContext;

    // Dependency Injection can be used here
    public TransactionBehaviour(DomainDbContext domainDbContext)
    {
        _domainDbContext = domainDbContext;
        Console.WriteLine("Initialized transaction behaviour");
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        return await next();

        #region -- Old Code --

        // TODO: Improve this. Not applicable in all situations! Disabling for now.
        // For example- We do not need transactions when reading from database
        //await using var transaction = await _domainDbContext.Database.BeginTransactionAsync(cancellationToken);

        //try
        //{
        //    var response = await next();
        //    await transaction.CommitAsync(cancellationToken);
        //    return response;
        //}
        //catch (Exception)
        //{
        //    await transaction.RollbackAsync(cancellationToken);
        //    throw;
        //}

        #endregion
    }
}