namespace Kairudev.Application.Tasks.ChangeTaskStatus;

public sealed record ChangeTaskStatusRequest(Guid TaskId, string NewStatus);
