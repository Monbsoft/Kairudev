using Kairudev.Application.Journal.CreateJournalEntry;
using Kairudev.Domain.Journal;
using Kairudev.Domain.Tasks;

namespace Kairudev.Application.Tasks.CompleteTask;

public sealed class CompleteTaskInteractor : ICompleteTaskUseCase
{
    private readonly ITaskRepository _repository;
    private readonly ICompleteTaskPresenter _presenter;
    private readonly ICreateJournalEntryUseCase _journalUseCase;

    public CompleteTaskInteractor(
        ITaskRepository repository,
        ICompleteTaskPresenter presenter,
        ICreateJournalEntryUseCase journalUseCase)
    {
        _repository = repository;
        _presenter = presenter;
        _journalUseCase = journalUseCase;
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

        await _journalUseCase.Execute(new CreateJournalEntryRequest(
            JournalEventType.TaskCompleted,
            task.Id.Value,
            task.CompletedAt!.Value), cancellationToken);

        _presenter.PresentSuccess();
    }
}
