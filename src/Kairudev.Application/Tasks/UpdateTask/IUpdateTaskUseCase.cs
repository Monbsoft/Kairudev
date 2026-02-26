namespace Kairudev.Application.Tasks.UpdateTask;

public interface IUpdateTaskUseCase
{
    Task Execute(UpdateTaskRequest request, CancellationToken cancellationToken = default);
}
