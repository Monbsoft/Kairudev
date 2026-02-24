namespace Kairudev.Application.Tasks.DeleteTask;

public interface IDeleteTaskUseCase
{
    Task Execute(DeleteTaskRequest request, CancellationToken cancellationToken = default);
}
