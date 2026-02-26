using Kairudev.Application.Journal.CreateJournalEntry;
using Kairudev.Domain.Journal;
using Kairudev.Domain.Pomodoro;
using PomodoroErrors = Kairudev.Domain.Pomodoro.DomainErrors;

namespace Kairudev.Application.Pomodoro.CompleteSession;

public sealed class CompleteSessionInteractor : ICompleteSessionUseCase
{
    private readonly IPomodoroSessionRepository _sessionRepository;
    private readonly IPomodoroSettingsRepository _settingsRepository;
    private readonly ICompleteSessionPresenter _presenter;
    private readonly ICreateJournalEntryUseCase _journalUseCase;

    public CompleteSessionInteractor(
        IPomodoroSessionRepository sessionRepository,
        IPomodoroSettingsRepository settingsRepository,
        ICompleteSessionPresenter presenter,
        ICreateJournalEntryUseCase journalUseCase)
    {
        _sessionRepository = sessionRepository;
        _settingsRepository = settingsRepository;
        _presenter = presenter;
        _journalUseCase = journalUseCase;
    }

    public async Task Execute(CompleteSessionRequest request, CancellationToken cancellationToken = default)
    {
        var session = await _sessionRepository.GetActiveAsync(cancellationToken);
        if (session is null)
        {
            _presenter.PresentFailure(PomodoroErrors.Pomodoro.SessionNotActive);
            return;
        }

        session.Complete(DateTime.UtcNow);
        await _sessionRepository.UpdateAsync(session, cancellationToken);

        await _journalUseCase.Execute(new CreateJournalEntryRequest(
            JournalEventType.SprintCompleted,
            session.Id.Value,
            session.EndedAt!.Value), cancellationToken);

        var completedCount = await _sessionRepository.GetCompletedTodayCountAsync(cancellationToken);
        var settings = await _settingsRepository.GetAsync(cancellationToken);

        var isLongBreak = completedCount % PomodoroSettings.SprintsBeforeLongBreak == 0;
        var breakType = isLongBreak ? "long" : "short";
        var breakDuration = isLongBreak
            ? settings.LongBreakDurationMinutes
            : settings.ShortBreakDurationMinutes;

        _presenter.PresentSuccess(new CompleteSessionResult(breakType, breakDuration));
    }
}
