using CleanArchitecture.DDD.Domain.ValueObjects;

namespace Domain.UnitTests.ValueObjects;

public class NameTests
{

    [Theory]
    [InlineData("Robert", "Mueller")]
    [InlineData("Don", "Herman")]
    // [ClassData(typeof(NameGenerator))]
    public void Verify_That_A_Name_Is_Valid_Without_A_Middlename(string firstname, string lastname)
    {
        var name = new Name(firstname, lastname);

        var validator = new NameValidator();
        var validationResult = validator.Validate(name);

        validationResult.IsValid.Should().BeTrue();
        name.ToString().Should().Be(firstname + " " + lastname);
    }

    [Theory]
    [InlineData("Robert1", "Mueller")]
    [InlineData("1Robert", "Mueller")]
    [InlineData("Rob1ert", "Mueller")]
    public void Verify_That_A_Name_Is_Not_Valid_When_Firstname_contains_a_number(string firstname, string lastname)
    {
        var name = new Name(firstname, lastname);

        var validator = new NameValidator();
        var validationResult = validator.Validate(name);

        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Count.Should().Be(1);
        
    }


}