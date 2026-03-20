namespace Kairudev.Application.Tasks.Queries.GetTaskById;

/// <summary>
/// Query to retrieve a single task by its identifier.
/// </summary>
public sealed record GetTaskByIdQuery(Guid Id);
