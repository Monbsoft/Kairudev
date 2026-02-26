using Kairudev.Domain.Common;

namespace Kairudev.Domain.Tasks;

public sealed class DeveloperTask : AggregateRoot<TaskId>
{
    private DeveloperTask(TaskId id, TaskTitle title, DateTime createdAt)
        : base(id)
    {
        Title = title;
        Status = TaskStatus.Pending;
        CreatedAt = createdAt;
    }

    public TaskTitle Title { get; private set; }
    public TaskStatus Status { get; private set; }
    public DateTime CreatedAt { get; }
    public DateTime? CompletedAt { get; private set; }

    public static DeveloperTask Create(TaskTitle title, DateTime createdAt) =>
        new(TaskId.New(), title, createdAt);

    public Result Complete()
    {
        if (Status == TaskStatus.Done)
            return Result.Failure(DomainErrors.Tasks.AlreadyCompleted);

        Status = TaskStatus.Done;
        CompletedAt = DateTime.UtcNow;
        return Result.Success();
    }

    public Result StartProgress()
    {
        if (Status == TaskStatus.Done)
            return Result.Failure(DomainErrors.Tasks.AlreadyCompleted);

        Status = TaskStatus.InProgress;
        return Result.Success();
    }

    public Result ChangeStatus(TaskStatus newStatus, DateTime now)
    {
        if (Status == newStatus)
            return Result.Failure(DomainErrors.Tasks.SameStatus);

        if (newStatus == TaskStatus.Done)
            CompletedAt = now;
        else if (Status == TaskStatus.Done)
            CompletedAt = null;

        Status = newStatus;
        return Result.Success();
    }
}
