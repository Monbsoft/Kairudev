namespace Kairudev.Application.Tasks.CompleteTask;

public interface ICompleteTaskPresenter
{
    void PresentSuccess();
    void PresentNotFound();
    void PresentFailure(string reason);
}
