using Kairudev.Application.Journal.CreateJournalEntry;
using Kairudev.Application.Tasks.Common;
using Kairudev.Domain.Journal;
using Kairudev.Domain.Tasks;
using DomainTaskStatus = Kairudev.Domain.Tasks.TaskStatus;

namespace Kairudev.Application.Tasks.ChangeTaskStatus;

public sealed class ChangeTaskStatusInteractor : IChangeTaskStatusUseCase
{
    private readonly ITaskRepository _repository;
    private readonly IChangeTaskStatusPresenter _presenter;
    private readonly ICreateJournalEntryUseCase _journalUseCase;

    public ChangeTaskStatusInteractor(
        ITaskRepository repository,
        IChangeTaskStatusPresenter presenter,
        ICreateJournalEntryUseCase journalUseCase)
    {
        _repository = repository;
        _presenter = presenter;
        _journalUseCase = journalUseCase;
    }

    public async Task Execute(ChangeTaskStatusRequest request, CancellationToken cancellationToken = default)
    {
        if (!Enum.TryParse<DomainTaskStatus>(request.NewStatus, ignoreCase: true, out var newStatus))
        {
            _presenter.PresentValidationError($"Unknown status value: '{request.NewStatus}'.");
            return;
        }

        var taskId = TaskId.From(request.TaskId);
        var task = await _repository.GetByIdAsync(taskId, cancellationToken);

        if (task is null)
        {
            _presenter.PresentNotFound();
            return;
        }

        var result = task.ChangeStatus(newStatus, DateTime.UtcNow);
        if (result.IsFailure)
        {
            _presenter.PresentFailure(result.Error);
            return;
        }

        await _repository.UpdateAsync(task, cancellationToken);

        if (newStatus == DomainTaskStatus.InProgress)
        {
            await _journalUseCase.Execute(new CreateJournalEntryRequest(
                JournalEventType.TaskStarted,
                task.Id.Value,
                DateTime.UtcNow), cancellationToken);
        }
        else if (newStatus == DomainTaskStatus.Done)
        {
            await _journalUseCase.Execute(new CreateJournalEntryRequest(
                JournalEventType.TaskCompleted,
                task.Id.Value,
                task.CompletedAt!.Value), cancellationToken);
        }

        _presenter.PresentSuccess(TaskViewModel.From(task));
    }
}
