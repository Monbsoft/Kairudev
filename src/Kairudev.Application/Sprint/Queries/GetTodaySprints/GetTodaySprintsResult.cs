using Kairudev.Application.Sprint.Common;

namespace Kairudev.Application.Sprint.Queries.GetTodaySprints;

public sealed record GetTodaySprintsResult(IReadOnlyList<SprintSessionViewModel> Sessions);
