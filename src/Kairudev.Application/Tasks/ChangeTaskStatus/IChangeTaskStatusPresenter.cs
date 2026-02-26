using Kairudev.Application.Tasks.Common;

namespace Kairudev.Application.Tasks.ChangeTaskStatus;

public interface IChangeTaskStatusPresenter
{
    void PresentSuccess(TaskViewModel task);
    void PresentNotFound();
    void PresentValidationError(string error);
    void PresentFailure(string reason);
}
