using Kairudev.Application.Tasks.AddTask;
using Kairudev.Application.Tasks.Common;
using Kairudev.Domain.Tasks;

namespace Kairudev.Application.Tests.Tasks;

public sealed class AddTaskInteractorTests
{
    private readonly FakeTaskRepository _repository = new();
    private readonly FakeAddTaskPresenter _presenter = new();
    private readonly AddTaskInteractor _sut;

    public AddTaskInteractorTests() =>
        _sut = new AddTaskInteractor(_repository, _presenter);

    [Fact]
    public async Task Should_PresentSuccess_When_TitleIsValid()
    {
        await _sut.Execute(new AddTaskRequest("Write documentation"));

        Assert.True(_presenter.IsSuccess);
        Assert.NotNull(_presenter.Task);
        Assert.Equal("Write documentation", _presenter.Task.Title);
        Assert.Equal("Pending", _presenter.Task.Status);
    }

    [Fact]
    public async Task Should_PersistTask_When_TitleIsValid()
    {
        await _sut.Execute(new AddTaskRequest("Write documentation"));

        Assert.Single(_repository.Tasks);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task Should_PresentValidationError_When_TitleIsEmpty(string title)
    {
        await _sut.Execute(new AddTaskRequest(title));

        Assert.False(_presenter.IsSuccess);
        Assert.NotNull(_presenter.ValidationError);
        Assert.Empty(_repository.Tasks);
    }

    [Fact]
    public async Task Should_PresentValidationError_When_TitleTooLong()
    {
        var longTitle = new string('a', TaskTitle.MaxLength + 1);

        await _sut.Execute(new AddTaskRequest(longTitle));

        Assert.False(_presenter.IsSuccess);
        Assert.NotNull(_presenter.ValidationError);
    }

    private sealed class FakeAddTaskPresenter : IAddTaskPresenter
    {
        public TaskViewModel? Task { get; private set; }
        public string? ValidationError { get; private set; }
        public string? FailureReason { get; private set; }
        public bool IsSuccess { get; private set; }

        public void PresentSuccess(TaskViewModel task) { Task = task; IsSuccess = true; }
        public void PresentValidationError(string error) => ValidationError = error;
        public void PresentFailure(string reason) => FailureReason = reason;
    }
}
