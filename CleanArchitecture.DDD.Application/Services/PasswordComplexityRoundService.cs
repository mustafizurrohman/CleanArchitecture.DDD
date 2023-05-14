namespace CleanArchitecture.DDD.Application.Services;

public class PasswordComplexityRoundService
    : IPasswordComplexityRoundService
{
    // This can be constant as it was implemented in 2023
    private static int BaseYear => 2023;
    
    private static int BaseYearRounds => 100_000;

    public int GetPasswordRounds()
    {
        var numberOfYearsFromBaseYear = DateTime.Now.Year - BaseYear;

        var factor = numberOfYearsFromBaseYear == 0
            ? 1
            : numberOfYearsFromBaseYear / 2;

        return BaseYearRounds * factor;
    }
}

