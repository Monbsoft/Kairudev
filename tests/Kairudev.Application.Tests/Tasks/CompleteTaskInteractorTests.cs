using Kairudev.Application.Journal.CreateJournalEntry;
using Kairudev.Application.Tasks.CompleteTask;
using Kairudev.Domain.Tasks;
using DomainTaskStatus = Kairudev.Domain.Tasks.TaskStatus;

namespace Kairudev.Application.Tests.Tasks;

public sealed class CompleteTaskInteractorTests
{
    private readonly FakeTaskRepository _repository = new();
    private readonly FakeCompleteTaskPresenter _presenter = new();
    private readonly NoOpCreateJournalEntryUseCase _noOpJournal = new();
    private readonly CompleteTaskInteractor _sut;

    public CompleteTaskInteractorTests() =>
        _sut = new CompleteTaskInteractor(_repository, _presenter, _noOpJournal);

    [Fact]
    public async Task Should_PresentSuccess_When_TaskExistsAndIsPending()
    {
        var task = CreateAndAddTask();

        await _sut.Execute(new CompleteTaskRequest(task.Id.Value));

        Assert.True(_presenter.IsSuccess);
        Assert.Equal(DomainTaskStatus.Done, _repository.Tasks[0].Status);
    }

    [Fact]
    public async Task Should_PresentNotFound_When_TaskDoesNotExist()
    {
        await _sut.Execute(new CompleteTaskRequest(Guid.NewGuid()));

        Assert.True(_presenter.IsNotFound);
        Assert.False(_presenter.IsSuccess);
    }

    [Fact]
    public async Task Should_PresentFailure_When_TaskAlreadyCompleted()
    {
        var task = CreateAndAddTask();
        task.Complete();

        await _sut.Execute(new CompleteTaskRequest(task.Id.Value));

        Assert.False(_presenter.IsSuccess);
        Assert.NotNull(_presenter.FailureReason);
    }

    private DeveloperTask CreateAndAddTask()
    {
        var task = DeveloperTask.Create(TaskTitle.Create("Task to complete").Value, null, DateTime.UtcNow);
        _repository.Tasks.Add(task);
        return task;
    }

    private sealed class NoOpCreateJournalEntryUseCase : ICreateJournalEntryUseCase
    {
        public Task Execute(CreateJournalEntryRequest request, CancellationToken cancellationToken = default) =>
            Task.CompletedTask;
    }

    private sealed class FakeCompleteTaskPresenter : ICompleteTaskPresenter
    {
        public bool IsSuccess { get; private set; }
        public bool IsNotFound { get; private set; }
        public string? FailureReason { get; private set; }

        public void PresentSuccess() => IsSuccess = true;
        public void PresentNotFound() => IsNotFound = true;
        public void PresentFailure(string reason) => FailureReason = reason;
    }
}
