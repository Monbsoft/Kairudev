using Kairudev.Application.Tasks.ChangeTaskStatus;
using Kairudev.Application.Tasks.Common;
using Microsoft.AspNetCore.Mvc;

namespace Kairudev.Api.Tasks.Presenters;

public sealed class ChangeTaskStatusHttpPresenter : IChangeTaskStatusPresenter
{
    public IActionResult? Result { get; private set; }

    public void PresentSuccess(TaskViewModel task) =>
        Result = new OkObjectResult(task);

    public void PresentNotFound() =>
        Result = new NotFoundResult();

    public void PresentValidationError(string error) =>
        Result = new BadRequestObjectResult(new { error });

    public void PresentFailure(string reason) =>
        Result = new ConflictObjectResult(new { error = reason });
}
