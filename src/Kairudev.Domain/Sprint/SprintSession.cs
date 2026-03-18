using Kairudev.Domain.Common;
using Kairudev.Domain.Identity;
using Kairudev.Domain.Tasks;

namespace Kairudev.Domain.Sprint;

public sealed class SprintSession : AggregateRoot<SprintSessionId>
{
    private SprintSession(
        SprintSessionId id,
        SprintName name,
        UserId ownerId,
        DateTimeOffset startedAt,
        DateTimeOffset endedAt,
        SprintOutcome outcome,
        TaskId? linkedTaskId)
        : base(id)
    {
        Name = name;
        OwnerId = ownerId;
        StartedAt = startedAt;
        EndedAt = endedAt;
        Outcome = outcome;
        LinkedTaskId = linkedTaskId;
    }

    // Public parameterless constructor for EF Core
    private SprintSession() : base(SprintSessionId.New()) { }

    public SprintName Name { get; private set; } = default!;
    public UserId OwnerId { get; private set; } = default!;
    public DateTimeOffset StartedAt { get; private set; }
    public DateTimeOffset EndedAt { get; private set; }
    public SprintOutcome Outcome { get; private set; }
    public TaskId? LinkedTaskId { get; private set; }

    /// <summary>Computed property — not persisted by EF Core.</summary>
    public TimeSpan Duration => EndedAt - StartedAt;

    public static Result<SprintSession> Record(
        SprintName name,
        UserId ownerId,
        DateTimeOffset startedAt,
        DateTimeOffset endedAt,
        SprintOutcome outcome,
        TaskId? linkedTaskId)
    {
        if (endedAt <= startedAt)
            return Result.Failure<SprintSession>(SprintDomainErrors.Sprint.EndedAtBeforeStartedAt);

        var session = new SprintSession(
            SprintSessionId.New(),
            name,
            ownerId,
            startedAt,
            endedAt,
            outcome,
            linkedTaskId);

        return Result.Success(session);
    }
}
