using Kairudev.Application.Common;
using Kairudev.Application.Journal.Commands.CreateEntry;
using Kairudev.Application.Sprint.Common;
using Kairudev.Domain.Journal;
using Kairudev.Domain.Sprint;
using Kairudev.Domain.Tasks;

namespace Kairudev.Application.Sprint.Commands.RecordSprint;

public sealed class RecordSprintCommandHandler
{
    private readonly ISprintSessionRepository _sprintRepository;
    private readonly CreateEntryCommandHandler _journalHandler;
    private readonly ICurrentUserService _currentUserService;

    public RecordSprintCommandHandler(
        ISprintSessionRepository sprintRepository,
        CreateEntryCommandHandler journalHandler,
        ICurrentUserService currentUserService)
    {
        _sprintRepository = sprintRepository;
        _journalHandler = journalHandler;
        _currentUserService = currentUserService;
    }

    public async Task<RecordSprintResult> HandleAsync(
        RecordSprintCommand command,
        CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.CurrentUserId;

        var nameResult = SprintName.Create(command.Name, command.SprintNumber);
        if (nameResult.IsFailure)
            return RecordSprintResult.Failure(nameResult.Error);

        if (!Enum.TryParse<SprintOutcome>(command.Outcome, true, out var outcome))
            return RecordSprintResult.Failure($"Invalid sprint outcome: '{command.Outcome}'. Expected 'Completed' or 'Interrupted'.");

        TaskId? linkedTaskId = command.LinkedTaskId.HasValue
            ? TaskId.From(command.LinkedTaskId.Value)
            : null;

        var sessionResult = SprintSession.Record(
            nameResult.Value,
            userId,
            command.StartedAt,
            command.EndedAt,
            outcome,
            linkedTaskId);

        if (sessionResult.IsFailure)
            return RecordSprintResult.Failure(sessionResult.Error);

        var session = sessionResult.Value;
        await _sprintRepository.AddAsync(session, cancellationToken);

        // Create retro-active journal entries
        await _journalHandler.HandleAsync(
            new CreateEntryCommand(
                JournalEventType.SprintStarted,
                session.Id.Value,
                command.StartedAt.UtcDateTime,
                userId),
            cancellationToken);

        var completionEventType = outcome == SprintOutcome.Completed
            ? JournalEventType.SprintCompleted
            : JournalEventType.SprintInterrupted;

        await _journalHandler.HandleAsync(
            new CreateEntryCommand(
                completionEventType,
                session.Id.Value,
                command.EndedAt.UtcDateTime,
                userId),
            cancellationToken);

        return RecordSprintResult.Success(SprintSessionViewModel.From(session));
    }
}
