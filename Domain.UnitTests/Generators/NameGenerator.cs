using System.Collections;
using Bogus;
using CleanArchitecture.DDD.Domain.ValueObjects;

namespace Domain.UnitTests.Generators;

public class NameGenerator : IEnumerable<IEnumerable<Name>>
{
    private readonly IEnumerable<Name> _names = GetNames();

    private static IEnumerable<Name> GetNames()
    {
        const int numberOfNames = 1000;

        var faker = new Faker();
        
        var names = Enumerable.Range(0, numberOfNames)
            .Select(_ => new Name(faker.Name.FirstName(), faker.Name.LastName()))
            .ToList();

        return names;
    }
    
    public IEnumerator<IEnumerable<Name>> GetEnumerator()
    {
        throw new NotImplementedException();
        // return _names.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}