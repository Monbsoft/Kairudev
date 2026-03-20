using Kairudev.Application.Common;
using Kairudev.Application.Tasks.Common;
using Kairudev.Domain.Tasks;

namespace Kairudev.Application.Tasks.Queries.GetTaskById;

/// <summary>
/// Handles the GetTaskById query.
/// </summary>
public sealed class GetTaskByIdQueryHandler
{
    private readonly ITaskRepository _repository;
    private readonly ICurrentUserService _currentUserService;

    public GetTaskByIdQueryHandler(ITaskRepository repository, ICurrentUserService currentUserService)
    {
        _repository = repository;
        _currentUserService = currentUserService;
    }

    public async Task<GetTaskByIdResult> HandleAsync(
        GetTaskByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.CurrentUserId;
        var taskId = TaskId.From(query.Id);
        var task = await _repository.GetByIdAsync(taskId, userId, cancellationToken);
        return new GetTaskByIdResult(task is null ? null : TaskViewModel.From(task));
    }
}
