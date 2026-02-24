using Kairudev.Application.Tasks.Common;
using Kairudev.Domain.Tasks;

namespace Kairudev.Application.Tasks.ListTasks;

public sealed class ListTasksInteractor : IListTasksUseCase
{
    private readonly ITaskRepository _repository;
    private readonly IListTasksPresenter _presenter;

    public ListTasksInteractor(ITaskRepository repository, IListTasksPresenter presenter)
    {
        _repository = repository;
        _presenter = presenter;
    }

    public async Task Execute(ListTasksRequest request, CancellationToken cancellationToken = default)
    {
        var tasks = await _repository.GetAllAsync(cancellationToken);
        var viewModels = tasks.Select(TaskViewModel.From).ToList().AsReadOnly();
        _presenter.PresentSuccess(viewModels);
    }
}
