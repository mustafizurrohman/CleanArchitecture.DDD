using CleanArchitecture.DDD.Infrastructure;
using CleanArchitecture.DDD.Infrastructure.Persistence.Entities;
using FluentAssertions;
using FluentAssertions.Types;

namespace CodeIntegrityTests;

public class TechnicalTests
{
    [Fact]
    public void VerifyThatAlEntityClassesAreSealedToEnsureBestPractice()
    {
        var assembly = typeof(InfrastructureAssemblyMarker).Assembly;

        var allEntityTypes = AllTypes.From(assembly)
            .ThatAreInNamespace("CleanArchitecture.DDD.Infrastructure.Persistence.Entities");
            // .Except(new List<Type>() {typeof(BaseEntity)});

        allEntityTypes
            .Should()
            .BeSealed();
    }
}