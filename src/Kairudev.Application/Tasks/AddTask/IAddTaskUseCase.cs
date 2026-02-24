namespace Kairudev.Application.Tasks.AddTask;

public interface IAddTaskUseCase
{
    Task Execute(AddTaskRequest request, CancellationToken cancellationToken = default);
}
