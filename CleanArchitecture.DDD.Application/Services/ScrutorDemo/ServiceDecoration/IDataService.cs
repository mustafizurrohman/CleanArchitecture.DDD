namespace CleanArchitecture.DDD.Application.Services.ScrutorDemo.ServiceDecoration;

public interface IDataService
{
    public Task<IEnumerable<DemoData>> GetDemoDataAsync(int num);
}