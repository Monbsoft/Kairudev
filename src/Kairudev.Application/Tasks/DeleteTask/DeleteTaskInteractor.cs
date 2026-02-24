using Kairudev.Domain.Tasks;

namespace Kairudev.Application.Tasks.DeleteTask;

public sealed class DeleteTaskInteractor : IDeleteTaskUseCase
{
    private readonly ITaskRepository _repository;
    private readonly IDeleteTaskPresenter _presenter;

    public DeleteTaskInteractor(ITaskRepository repository, IDeleteTaskPresenter presenter)
    {
        _repository = repository;
        _presenter = presenter;
    }

    public async Task Execute(DeleteTaskRequest request, CancellationToken cancellationToken = default)
    {
        var taskId = TaskId.From(request.TaskId);
        var task = await _repository.GetByIdAsync(taskId, cancellationToken);

        if (task is null)
        {
            _presenter.PresentNotFound();
            return;
        }

        await _repository.DeleteAsync(taskId, cancellationToken);
        _presenter.PresentSuccess();
    }
}
