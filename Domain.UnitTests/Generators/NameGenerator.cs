using System.Collections;
using Bogus;
using CleanArchitecture.DDD.Domain.ValueObjects;

namespace Domain.UnitTests.Generators;

public class NameGenerator : IEnumerable<Name>
{
    private readonly IEnumerable<Name> _names = GetNames();

    private static IEnumerable<Name> GetNames()
    {
        const int numberOfNames = 1000;

        var faker = new Faker();
        
        var names = Enumerable.Range(0, numberOfNames)
            .Select(_ => Name.Create(faker.Name.FirstName(), string.Empty, faker.Name.LastName()))
            .ToList();

        return names;
    }

    public IEnumerator<Name> GetEnumerator()
    {
        return _names.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}