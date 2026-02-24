using Kairudev.Application.Tasks.AddTask;
using Kairudev.Application.Tasks.Common;

namespace Kairudev.Adapters.Tasks;

public sealed class AddTaskPresenter : IAddTaskPresenter
{
    public TaskViewModel? Task { get; private set; }
    public string? ValidationError { get; private set; }
    public string? FailureReason { get; private set; }
    public bool IsSuccess { get; private set; }

    public void PresentSuccess(TaskViewModel task)
    {
        Task = task;
        IsSuccess = true;
    }

    public void PresentValidationError(string error) =>
        ValidationError = error;

    public void PresentFailure(string reason) =>
        FailureReason = reason;
}
