using Kairudev.Application.Common;
using Kairudev.Application.Sprint.Common;
using Kairudev.Domain.Sprint;

namespace Kairudev.Application.Sprint.Queries.GetTodaySprints;

public sealed class GetTodaySprintsQueryHandler
{
    private readonly ISprintSessionRepository _sprintRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetTodaySprintsQueryHandler(
        ISprintSessionRepository sprintRepository,
        ICurrentUserService currentUserService)
    {
        _sprintRepository = sprintRepository;
        _currentUserService = currentUserService;
    }

    public async Task<GetTodaySprintsResult> HandleAsync(
        GetTodaySprintsQuery query,
        CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.CurrentUserId;
        var today = DateOnly.FromDateTime(DateTimeOffset.UtcNow.Date);

        var sessions = await _sprintRepository.GetByDateAsync(today, userId, cancellationToken);

        var viewModels = sessions
            .OrderBy(s => s.StartedAt)
            .Select(SprintSessionViewModel.From)
            .ToList();

        return new GetTodaySprintsResult(viewModels);
    }
}
