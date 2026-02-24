namespace Kairudev.Application.Tasks.DeleteTask;

public interface IDeleteTaskPresenter
{
    void PresentSuccess();
    void PresentNotFound();
    void PresentFailure(string reason);
}
