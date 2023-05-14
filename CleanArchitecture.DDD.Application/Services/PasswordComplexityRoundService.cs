namespace CleanArchitecture.DDD.Application.Services;

public class PasswordComplexityRoundService
    : IPasswordComplexityRoundService
{
    // This can be constant as it was implemented in 2023 or adjusted along with BaseYearRounds
    private static int BaseYear => 2023;
    
    // This can be rechecked and both BaseYear and BaseYearRounds can be adjusted at the same time
    private static int BaseYearRounds => 100_000;

    public int GetPasswordRounds()
    {
        double numberOfYearsFromBaseYear = DateTime.Now.Year - BaseYear;

        var factor = numberOfYearsFromBaseYear == 0
            ? 1
            : (int)Math.Ceiling(numberOfYearsFromBaseYear / 2);

        return BaseYearRounds * factor;
    }
}

