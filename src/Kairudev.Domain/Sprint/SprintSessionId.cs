namespace Kairudev.Domain.Sprint;

public sealed record SprintSessionId(Guid Value)
{
    public static SprintSessionId New() => new(Guid.NewGuid());
    public static SprintSessionId From(Guid value) => new(value);

    public override string ToString() => Value.ToString();
}
