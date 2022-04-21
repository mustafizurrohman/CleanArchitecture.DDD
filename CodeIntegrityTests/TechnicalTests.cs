using CleanArchitecture.DDD.Infrastructure;
using FluentAssertions;
using FluentAssertions.Types;

namespace CodeIntegrityTests
{
    public class TechnicalTests
    {
        [Fact]
        public void VerifyThatAlEntityClassesAreSealedToEnsureBestPractice()
        {
            var assembly = typeof(InfrastructureAssemblyMarker).Assembly;

            var allEntityTypes = AllTypes.From(assembly)
                .ThatAreInNamespace("CleanArchitecture.DDD.Infrastructure.Persistence.Entities");
            
            foreach (var entityType in allEntityTypes)
            {
                entityType
                    .Should()
                    .BeSealed();
            }
        }
    }
}