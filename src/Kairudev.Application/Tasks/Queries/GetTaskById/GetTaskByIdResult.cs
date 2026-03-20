using Kairudev.Application.Tasks.Common;

namespace Kairudev.Application.Tasks.Queries.GetTaskById;

/// <summary>
/// Result of the GetTaskById query. Task is null when not found.
/// </summary>
public sealed record GetTaskByIdResult(TaskViewModel? Task);
