using Kairudev.Application.Tasks.Common;

namespace Kairudev.Application.Tasks.ListTasks;

public interface IListTasksPresenter
{
    void PresentSuccess(IReadOnlyList<TaskViewModel> tasks);
    void PresentFailure(string reason);
}
