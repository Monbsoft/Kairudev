using Kairudev.Domain.Common;

namespace Kairudev.Domain.Sprint;

public sealed record SprintName
{
    public const int MaxLength = 200;

    public string Value { get; }

    private SprintName(string value) => Value = value;

    /// <summary>
    /// Creates a SprintName. If value is empty or whitespace, uses the default name "Sprint #N"
    /// where N is the sprintNumber parameter.
    /// </summary>
    public static Result<SprintName> Create(string? value, int sprintNumber)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result.Success(new SprintName($"Sprint #{sprintNumber}"));

        if (value.Length > MaxLength)
            return Result.Failure<SprintName>(SprintDomainErrors.Sprint.NameTooLong);

        return Result.Success(new SprintName(value));
    }
}
