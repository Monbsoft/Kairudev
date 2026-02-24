using Kairudev.Application.Tasks.Common;
using Kairudev.Application.Tasks.ListTasks;
using Microsoft.AspNetCore.Mvc;

namespace Kairudev.Api.Tasks.Presenters;

public sealed class ListTasksHttpPresenter : IListTasksPresenter
{
    public IActionResult? Result { get; private set; }

    public void PresentSuccess(IReadOnlyList<TaskViewModel> tasks) =>
        Result = new OkObjectResult(tasks);

    public void PresentFailure(string reason) =>
        Result = new ObjectResult(new { error = reason }) { StatusCode = 500 };
}
