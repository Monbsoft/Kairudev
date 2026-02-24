using Kairudev.Domain.Tasks;

namespace Kairudev.Application.Tasks.CompleteTask;

public sealed class CompleteTaskInteractor : ICompleteTaskUseCase
{
    private readonly ITaskRepository _repository;
    private readonly ICompleteTaskPresenter _presenter;

    public CompleteTaskInteractor(ITaskRepository repository, ICompleteTaskPresenter presenter)
    {
        _repository = repository;
        _presenter = presenter;
    }

    public async Task Execute(CompleteTaskRequest request, CancellationToken cancellationToken = default)
    {
        var taskId = TaskId.From(request.TaskId);
        var task = await _repository.GetByIdAsync(taskId, cancellationToken);

        if (task is null)
        {
            _presenter.PresentNotFound();
            return;
        }

        var result = task.Complete();
        if (result.IsFailure)
        {
            _presenter.PresentFailure(result.Error);
            return;
        }

        await _repository.UpdateAsync(task, cancellationToken);
        _presenter.PresentSuccess();
    }
}
