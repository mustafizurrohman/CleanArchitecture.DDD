using CleanArchitecture.DDD.Domain.ValueObjects;
using FluentValidation;

namespace Domain.UnitTests.ValueObjects;

public class NameTests
{

    [Theory]
    [InlineData("Robert", "Mueller")]
    [InlineData("Don", "Herman")]
    // [ClassData(typeof(NameGenerator))]
    public void Verify_That_A_Name_Is_Valid_Without_A_Middlename(string firstname, string lastname)
    {
        var name = Name.Create(firstname, lastname);
        name.ToString().Should().Be(firstname + " " + lastname);
    }

    [Theory]
    [InlineData("Robert1", "Mueller")]
    [InlineData("1Robert", "Mueller")]
    [InlineData("Rob1ert", "Mueller")]
    public void Verify_That_A_Name_Is_Not_Valid_When_Firstname_contains_a_number(string firstname, string lastname)
    {
        Action operation = () => Name.Create(firstname, lastname);
        operation.Should().Throw<ValidationException>();
    }


}