using Kairudev.Application.Tasks.CompleteTask;
using Microsoft.AspNetCore.Mvc;

namespace Kairudev.Api.Tasks.Presenters;

public sealed class CompleteTaskHttpPresenter : ICompleteTaskPresenter
{
    public IActionResult? Result { get; private set; }

    public void PresentSuccess() =>
        Result = new NoContentResult();

    public void PresentNotFound() =>
        Result = new NotFoundResult();

    public void PresentFailure(string reason) =>
        Result = new BadRequestObjectResult(new { error = reason });
}
