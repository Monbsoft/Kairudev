namespace Kairudev.Application.Tasks.CompleteTask;

public interface ICompleteTaskUseCase
{
    Task Execute(CompleteTaskRequest request, CancellationToken cancellationToken = default);
}
