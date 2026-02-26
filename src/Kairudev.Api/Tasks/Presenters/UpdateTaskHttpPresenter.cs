using Kairudev.Application.Tasks.Common;
using Kairudev.Application.Tasks.UpdateTask;
using Microsoft.AspNetCore.Mvc;

namespace Kairudev.Api.Tasks.Presenters;

public sealed class UpdateTaskHttpPresenter : IUpdateTaskPresenter
{
    public IActionResult? Result { get; private set; }

    public void PresentSuccess(TaskViewModel task) =>
        Result = new OkObjectResult(task);

    public void PresentValidationError(string error) =>
        Result = new BadRequestObjectResult(new { error });

    public void PresentNotFound() =>
        Result = new NotFoundResult();
}
