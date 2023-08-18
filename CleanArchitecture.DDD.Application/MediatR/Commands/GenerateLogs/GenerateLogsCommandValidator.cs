namespace CleanArchitecture.DDD.Application.MediatR.Commands.GenerateLogs;

public sealed class GenerateLogsCommandValidator : AbstractValidator<GenerateLogsCommand>
{
    public GenerateLogsCommandValidator()
    {
        RuleFor(cmd => cmd.Iterations)
            .GreaterThanOrEqualTo(1);
    }
}

