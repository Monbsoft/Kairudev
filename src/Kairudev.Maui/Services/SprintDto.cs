namespace Kairudev.Maui.Services;

public sealed record SprintSessionDto(
    Guid Id,
    string Name,
    DateTimeOffset StartedAt,
    DateTimeOffset EndedAt,
    string Outcome,
    double DurationSeconds,
    Guid? LinkedTaskId);

public sealed record RecordSprintRequest(
    string? Note,
    DateTimeOffset StartedAt,
    DateTimeOffset EndedAt,
    string Outcome,
    Guid? LinkedTaskId,
    int SprintNumber);
