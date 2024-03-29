﻿namespace CleanArchitecture.DDD.Application.Services.ScrutorDemo.ServiceDecoration;

public class DataService : IDataService
{
    public DataService()
    {
        Log.Information("DataService REAL- Initialized service...");
    }

    public Task<IEnumerable<DemoData>> GetDemoDataAsync(int num)
    {
        num = Guard.Against.NegativeOrZero(num);

        var demoDataFaker = new Faker<DemoData>()
            .StrictMode(true)
            .RuleFor(da => da.CreatedDateTime, _ => DateTime.Now)
            .RuleFor(da => da.Firstname, fake => fake.Name.FirstName())
            .RuleFor(da => da.Lastname, fake => fake.Name.LastName())
            .RuleFor(da => da.Cached, _ => false);

        Log.Information("Generating {numberOfGeneratedDemoData} DemoData ... ", num);

        return Task.FromResult(demoDataFaker.Generate(num).AsEnumerable());
    }
}