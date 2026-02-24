using Kairudev.Application.Tasks.Common;
using Kairudev.Domain.Tasks;

namespace Kairudev.Application.Tasks.AddTask;

public sealed class AddTaskInteractor : IAddTaskUseCase
{
    private readonly ITaskRepository _repository;
    private readonly IAddTaskPresenter _presenter;

    public AddTaskInteractor(ITaskRepository repository, IAddTaskPresenter presenter)
    {
        _repository = repository;
        _presenter = presenter;
    }

    public async Task Execute(AddTaskRequest request, CancellationToken cancellationToken = default)
    {
        var titleResult = TaskTitle.Create(request.Title);
        if (titleResult.IsFailure)
        {
            _presenter.PresentValidationError(titleResult.Error);
            return;
        }

        var task = DeveloperTask.Create(titleResult.Value, DateTime.UtcNow);
        await _repository.AddAsync(task, cancellationToken);
        _presenter.PresentSuccess(TaskViewModel.From(task));
    }
}
