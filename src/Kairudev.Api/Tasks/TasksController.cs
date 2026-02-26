using Kairudev.Api.Tasks.Presenters;
using Kairudev.Application.Journal.CreateJournalEntry;
using Kairudev.Application.Tasks.AddTask;
using Kairudev.Application.Tasks.ChangeTaskStatus;
using Kairudev.Application.Tasks.CompleteTask;
using Kairudev.Application.Tasks.DeleteTask;
using Kairudev.Application.Tasks.ListTasks;
using Kairudev.Domain.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Kairudev.Api.Tasks;

[ApiController]
[Route("api/tasks")]
public sealed class TasksController : ControllerBase
{
    private readonly ITaskRepository _repository;
    private readonly ICreateJournalEntryUseCase _journalUseCase;

    public TasksController(ITaskRepository repository, ICreateJournalEntryUseCase journalUseCase)
    {
        _repository = repository;
        _journalUseCase = journalUseCase;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var presenter = new ListTasksHttpPresenter();
        await new ListTasksInteractor(_repository, presenter)
            .Execute(new ListTasksRequest(), cancellationToken);
        return presenter.Result!;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] AddTaskRequest request,
        CancellationToken cancellationToken)
    {
        var presenter = new AddTaskHttpPresenter();
        await new AddTaskInteractor(_repository, presenter)
            .Execute(request, cancellationToken);
        return presenter.Result!;
    }

    [HttpPut("{id:guid}/complete")]
    public async Task<IActionResult> Complete(Guid id, CancellationToken cancellationToken)
    {
        var presenter = new CompleteTaskHttpPresenter();
        await new CompleteTaskInteractor(_repository, presenter, _journalUseCase)
            .Execute(new CompleteTaskRequest(id), cancellationToken);
        return presenter.Result!;
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var presenter = new DeleteTaskHttpPresenter();
        await new DeleteTaskInteractor(_repository, presenter)
            .Execute(new DeleteTaskRequest(id), cancellationToken);
        return presenter.Result!;
    }

    [HttpPatch("{id:guid}/status")]
    public async Task<IActionResult> ChangeStatus(
        Guid id,
        [FromBody] ChangeTaskStatusBody body,
        CancellationToken cancellationToken)
    {
        var presenter = new ChangeTaskStatusHttpPresenter();
        await new ChangeTaskStatusInteractor(_repository, presenter, _journalUseCase)
            .Execute(new ChangeTaskStatusRequest(id, body.NewStatus), cancellationToken);
        return presenter.Result!;
    }
}

public sealed record ChangeTaskStatusBody(string NewStatus);
