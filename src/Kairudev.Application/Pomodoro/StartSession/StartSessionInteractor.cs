using Kairudev.Application.Journal.CreateJournalEntry;
using Kairudev.Application.Pomodoro.Common;
using Kairudev.Domain.Journal;
using Kairudev.Domain.Pomodoro;
using PomodoroErrors = Kairudev.Domain.Pomodoro.DomainErrors;

namespace Kairudev.Application.Pomodoro.StartSession;

public sealed class StartSessionInteractor : IStartSessionUseCase
{
    private readonly IPomodoroSessionRepository _sessionRepository;
    private readonly IPomodoroSettingsRepository _settingsRepository;
    private readonly IStartSessionPresenter _presenter;
    private readonly ICreateJournalEntryUseCase _journalUseCase;

    public StartSessionInteractor(
        IPomodoroSessionRepository sessionRepository,
        IPomodoroSettingsRepository settingsRepository,
        IStartSessionPresenter presenter,
        ICreateJournalEntryUseCase journalUseCase)
    {
        _sessionRepository = sessionRepository;
        _settingsRepository = settingsRepository;
        _presenter = presenter;
        _journalUseCase = journalUseCase;
    }

    public async Task Execute(StartSessionRequest request, CancellationToken cancellationToken = default)
    {
        var existing = await _sessionRepository.GetActiveAsync(cancellationToken);
        if (existing is not null)
        {
            _presenter.PresentFailure(PomodoroErrors.Pomodoro.SessionAlreadyActive);
            return;
        }

        var settings = await _settingsRepository.GetAsync(cancellationToken);
        var session = PomodoroSession.Create(settings.SprintDurationMinutes);
        session.Start(DateTime.UtcNow);

        await _sessionRepository.AddAsync(session, cancellationToken);

        await _journalUseCase.Execute(new CreateJournalEntryRequest(
            JournalEventType.SprintStarted,
            session.Id.Value,
            session.StartedAt!.Value), cancellationToken);

        _presenter.PresentSuccess(PomodoroSessionViewModel.From(session));
    }
}
