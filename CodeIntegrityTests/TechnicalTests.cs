using CleanArchitecture.DDD.Infrastructure;
using FluentAssertions.Types;

namespace CodeIntegrityTests;

public class TechnicalTests
{
    [Fact]
    public void VerifyThatAlEntityClassesAreSealedToEnsureBestPractice()
    {
        var assembly = typeof(IInfrastructureAssemblyMarker).Assembly;

        var allEntityTypes = AllTypes.From(assembly)
            .ThatAreInNamespace("CleanArchitecture.DDD.Infrastructure.Persistence.Entities");
            
        allEntityTypes
            .Should()
            .BeSealed();
    }
}