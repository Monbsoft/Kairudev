using Kairudev.Domain.Sprint;

namespace Kairudev.Application.Sprint.Common;

public sealed record SprintSessionViewModel(
    Guid Id,
    string Name,
    DateTimeOffset StartedAt,
    DateTimeOffset EndedAt,
    string Outcome,
    double DurationSeconds,
    Guid? LinkedTaskId)
{
    public static SprintSessionViewModel From(SprintSession session) =>
        new(
            session.Id.Value,
            session.Name.Value,
            session.StartedAt,
            session.EndedAt,
            session.Outcome.ToString(),
            session.Duration.TotalSeconds,
            session.LinkedTaskId?.Value);
}
