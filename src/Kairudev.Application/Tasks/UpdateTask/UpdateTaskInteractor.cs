using Kairudev.Application.Tasks.Common;
using Kairudev.Domain.Tasks;

namespace Kairudev.Application.Tasks.UpdateTask;

public sealed class UpdateTaskInteractor : IUpdateTaskUseCase
{
    private readonly ITaskRepository _repository;
    private readonly IUpdateTaskPresenter _presenter;

    public UpdateTaskInteractor(ITaskRepository repository, IUpdateTaskPresenter presenter)
    {
        _repository = repository;
        _presenter = presenter;
    }

    public async Task Execute(UpdateTaskRequest request, CancellationToken cancellationToken = default)
    {
        var taskId = TaskId.From(request.Id);
        var task = await _repository.GetByIdAsync(taskId, cancellationToken);

        if (task is null)
        {
            _presenter.PresentNotFound();
            return;
        }

        var titleResult = TaskTitle.Create(request.Title);
        if (titleResult.IsFailure)
        {
            _presenter.PresentValidationError(titleResult.Error);
            return;
        }

        var descriptionResult = TaskDescription.Create(request.Description);
        if (descriptionResult.IsFailure)
        {
            _presenter.PresentValidationError(descriptionResult.Error);
            return;
        }

        task.UpdateDetails(titleResult.Value, descriptionResult.Value);
        await _repository.UpdateAsync(task, cancellationToken);
        _presenter.PresentSuccess(TaskViewModel.From(task));
    }
}
