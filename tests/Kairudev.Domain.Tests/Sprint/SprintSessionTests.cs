using Kairudev.Domain.Identity;
using Kairudev.Domain.Sprint;
using Kairudev.Domain.Tasks;
using Xunit;

namespace Kairudev.Domain.Tests.Sprint;

public sealed class SprintSessionTests
{
    private static readonly UserId DefaultOwnerId = UserId.From(new Guid("00000000-0000-0000-0000-000000000001"));
    private static readonly DateTimeOffset StartedAt = new(2026, 3, 18, 9, 0, 0, TimeSpan.Zero);
    private static readonly DateTimeOffset EndedAt = new(2026, 3, 18, 9, 30, 0, TimeSpan.Zero);

    private static SprintName ValidName() => SprintName.Create("Focus", 1).Value;

    [Fact]
    public void Should_RecordSession_When_AllParametersAreValid()
    {
        var result = SprintSession.Record(ValidName(), DefaultOwnerId, StartedAt, EndedAt, SprintOutcome.Completed, null);

        Assert.True(result.IsSuccess);
        var session = result.Value;
        Assert.Equal("Focus", session.Name.Value);
        Assert.Equal(DefaultOwnerId, session.OwnerId);
        Assert.Equal(StartedAt, session.StartedAt);
        Assert.Equal(EndedAt, session.EndedAt);
        Assert.Equal(SprintOutcome.Completed, session.Outcome);
        Assert.Null(session.LinkedTaskId);
    }

    [Fact]
    public void Should_ComputeDurationCorrectly_When_SessionIsRecorded()
    {
        var result = SprintSession.Record(ValidName(), DefaultOwnerId, StartedAt, EndedAt, SprintOutcome.Completed, null);

        Assert.True(result.IsSuccess);
        Assert.Equal(TimeSpan.FromMinutes(30), result.Value.Duration);
    }

    [Fact]
    public void Should_RecordSession_When_OutcomeIsInterrupted()
    {
        var result = SprintSession.Record(ValidName(), DefaultOwnerId, StartedAt, EndedAt, SprintOutcome.Interrupted, null);

        Assert.True(result.IsSuccess);
        Assert.Equal(SprintOutcome.Interrupted, result.Value.Outcome);
    }

    [Fact]
    public void Should_RecordSession_When_LinkedTaskIdIsProvided()
    {
        var taskId = TaskId.New();
        var result = SprintSession.Record(ValidName(), DefaultOwnerId, StartedAt, EndedAt, SprintOutcome.Completed, taskId);

        Assert.True(result.IsSuccess);
        Assert.Equal(taskId, result.Value.LinkedTaskId);
    }

    [Fact]
    public void Should_Fail_When_EndedAtIsBeforeStartedAt()
    {
        var result = SprintSession.Record(ValidName(), DefaultOwnerId, EndedAt, StartedAt, SprintOutcome.Completed, null);

        Assert.True(result.IsFailure);
        Assert.Equal(SprintDomainErrors.Sprint.EndedAtBeforeStartedAt, result.Error);
    }

    [Fact]
    public void Should_Fail_When_EndedAtEqualsStartedAt()
    {
        var result = SprintSession.Record(ValidName(), DefaultOwnerId, StartedAt, StartedAt, SprintOutcome.Completed, null);

        Assert.True(result.IsFailure);
        Assert.Equal(SprintDomainErrors.Sprint.EndedAtBeforeStartedAt, result.Error);
    }

    [Fact]
    public void Should_GenerateUniqueIds_When_RecordingMultipleSessions()
    {
        var session1 = SprintSession.Record(ValidName(), DefaultOwnerId, StartedAt, EndedAt, SprintOutcome.Completed, null).Value;
        var session2 = SprintSession.Record(ValidName(), DefaultOwnerId, StartedAt, EndedAt, SprintOutcome.Completed, null).Value;

        Assert.NotEqual(session1.Id, session2.Id);
    }
}
