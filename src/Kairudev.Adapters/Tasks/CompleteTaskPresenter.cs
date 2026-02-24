using Kairudev.Application.Tasks.CompleteTask;

namespace Kairudev.Adapters.Tasks;

public sealed class CompleteTaskPresenter : ICompleteTaskPresenter
{
    public bool IsSuccess { get; private set; }
    public bool IsNotFound { get; private set; }
    public string? FailureReason { get; private set; }

    public void PresentSuccess() => IsSuccess = true;
    public void PresentNotFound() => IsNotFound = true;
    public void PresentFailure(string reason) => FailureReason = reason;
}
