namespace Kairudev.Application.Tasks.AddTask;

public sealed record AddTaskRequest(string Title, string? Description = null);
