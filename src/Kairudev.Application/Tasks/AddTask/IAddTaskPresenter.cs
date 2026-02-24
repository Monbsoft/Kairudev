using Kairudev.Application.Tasks.Common;

namespace Kairudev.Application.Tasks.AddTask;

public interface IAddTaskPresenter
{
    void PresentSuccess(TaskViewModel task);
    void PresentValidationError(string error);
    void PresentFailure(string reason);
}
