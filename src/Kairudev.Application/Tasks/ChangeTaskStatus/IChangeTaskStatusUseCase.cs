namespace Kairudev.Application.Tasks.ChangeTaskStatus;

public interface IChangeTaskStatusUseCase
{
    Task Execute(ChangeTaskStatusRequest request, CancellationToken cancellationToken = default);
}
