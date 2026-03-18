using Kairudev.Application.Sprint.Common;

namespace Kairudev.Application.Sprint.Commands.RecordSprint;

public sealed class RecordSprintResult
{
    public bool IsSuccess { get; private init; }
    public string? Error { get; private init; }
    public SprintSessionViewModel? Session { get; private init; }

    public static RecordSprintResult Success(SprintSessionViewModel session) =>
        new() { IsSuccess = true, Session = session };

    public static RecordSprintResult Failure(string error) =>
        new() { IsSuccess = false, Error = error };
}
