using Kairudev.Application.Tasks.AddTask;
using Kairudev.Application.Tasks.Common;
using Microsoft.AspNetCore.Mvc;

namespace Kairudev.Api.Tasks.Presenters;

public sealed class AddTaskHttpPresenter : IAddTaskPresenter
{
    public IActionResult? Result { get; private set; }

    public void PresentSuccess(TaskViewModel task) =>
        Result = new CreatedAtActionResult(
            actionName: "GetById",
            controllerName: "Tasks",
            routeValues: new { id = task.Id },
            value: task);

    public void PresentValidationError(string error) =>
        Result = new BadRequestObjectResult(new { error });

    public void PresentFailure(string reason) =>
        Result = new ObjectResult(new { error = reason }) { StatusCode = 500 };
}
