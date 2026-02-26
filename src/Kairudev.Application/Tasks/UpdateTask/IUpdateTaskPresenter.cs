using Kairudev.Application.Tasks.Common;

namespace Kairudev.Application.Tasks.UpdateTask;

public interface IUpdateTaskPresenter
{
    void PresentValidationError(string error);
    void PresentNotFound();
    void PresentSuccess(TaskViewModel task);
}
