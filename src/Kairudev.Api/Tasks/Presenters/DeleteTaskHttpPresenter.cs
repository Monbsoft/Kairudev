using Kairudev.Application.Tasks.DeleteTask;
using Microsoft.AspNetCore.Mvc;

namespace Kairudev.Api.Tasks.Presenters;

public sealed class DeleteTaskHttpPresenter : IDeleteTaskPresenter
{
    public IActionResult? Result { get; private set; }

    public void PresentSuccess() =>
        Result = new NoContentResult();

    public void PresentNotFound() =>
        Result = new NotFoundResult();

    public void PresentFailure(string reason) =>
        Result = new ObjectResult(new { error = reason }) { StatusCode = 500 };
}
