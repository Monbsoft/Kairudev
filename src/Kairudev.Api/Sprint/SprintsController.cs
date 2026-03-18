using Kairudev.Application.Sprint.Commands.RecordSprint;
using Kairudev.Application.Sprint.Queries.GetTodaySprints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kairudev.Api.Sprint;

[ApiController]
[Route("api/sprints")]
[Authorize]
public sealed class SprintsController : ControllerBase
{
    private readonly RecordSprintCommandHandler _recordSprint;
    private readonly GetTodaySprintsQueryHandler _getTodaySprints;

    public SprintsController(
        RecordSprintCommandHandler recordSprint,
        GetTodaySprintsQueryHandler getTodaySprints)
    {
        _recordSprint = recordSprint;
        _getTodaySprints = getTodaySprints;
    }

    /// <summary>Records a completed or interrupted sprint session.</summary>
    [HttpPost]
    public async Task<IActionResult> RecordSprint(
        [FromBody] RecordSprintBody body,
        CancellationToken ct)
    {
        var command = new RecordSprintCommand(
            body.Name,
            body.StartedAt,
            body.EndedAt,
            body.Outcome,
            body.LinkedTaskId,
            body.SprintNumber);

        var result = await _recordSprint.HandleAsync(command, ct);

        return result.IsSuccess
            ? Created($"api/sprints", result.Session)
            : BadRequest(new { error = result.Error });
    }

    /// <summary>Returns all sprint sessions recorded today for the current user.</summary>
    [HttpGet("today")]
    public async Task<IActionResult> GetTodaySprints(CancellationToken ct)
    {
        var result = await _getTodaySprints.HandleAsync(new GetTodaySprintsQuery(), ct);
        return Ok(result.Sessions);
    }
}

public sealed record RecordSprintBody(
    string? Name,
    DateTimeOffset StartedAt,
    DateTimeOffset EndedAt,
    string Outcome,
    Guid? LinkedTaskId,
    int SprintNumber);
