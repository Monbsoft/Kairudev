using Kairudev.Application.Tasks.Common;
using Kairudev.Application.Tasks.ListTasks;

namespace Kairudev.Adapters.Tasks;

public sealed class ListTasksPresenter : IListTasksPresenter
{
    public IReadOnlyList<TaskViewModel>? Tasks { get; private set; }
    public string? FailureReason { get; private set; }
    public bool IsSuccess { get; private set; }

    public void PresentSuccess(IReadOnlyList<TaskViewModel> tasks)
    {
        Tasks = tasks;
        IsSuccess = true;
    }

    public void PresentFailure(string reason) =>
        FailureReason = reason;
}
