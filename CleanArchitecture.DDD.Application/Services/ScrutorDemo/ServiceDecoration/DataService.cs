using System;

namespace CleanArchitecture.DDD.Application.Services.ScrutorDemo.ServiceDecoration;

public class DataService : IDataService
{
    public DataService()
    {
        Log.Information("DataService REAL- Initialized service...");
    }

    public Task<IEnumerable<DemoData>> GetDemoDataAsync(int num)
    {
        num = Guard.Against.NegativeOrZero(num, nameof(num));

        var demoDataFaker = new Faker<DemoData>()
            .StrictMode(true)
            .RuleFor(da => da.CreatedDateTime, _ => DateTime.Now)
            .RuleFor(da => da.Firstname, fake => fake.Name.FirstName())
            .RuleFor(da => da.Lastname, fake => fake.Name.LastName())
            .RuleFor(da => da.Cached, _ => false);

        return Task.FromResult(demoDataFaker.Generate(num).AsEnumerable());
    }
}