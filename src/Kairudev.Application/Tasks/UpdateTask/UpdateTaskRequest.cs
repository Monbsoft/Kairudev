namespace Kairudev.Application.Tasks.UpdateTask;

public sealed record UpdateTaskRequest(Guid Id, string Title, string? Description = null);
