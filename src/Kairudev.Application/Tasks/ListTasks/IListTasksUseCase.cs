namespace Kairudev.Application.Tasks.ListTasks;

public interface IListTasksUseCase
{
    Task Execute(ListTasksRequest request, CancellationToken cancellationToken = default);
}
