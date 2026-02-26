using Kairudev.Application.Journal.CreateJournalEntry;
using Kairudev.Domain.Journal;
using Kairudev.Domain.Pomodoro;
using PomodoroErrors = Kairudev.Domain.Pomodoro.DomainErrors;

namespace Kairudev.Application.Pomodoro.InterruptSession;

public sealed class InterruptSessionInteractor : IInterruptSessionUseCase
{
    private readonly IPomodoroSessionRepository _repository;
    private readonly IInterruptSessionPresenter _presenter;
    private readonly ICreateJournalEntryUseCase _journalUseCase;

    public InterruptSessionInteractor(
        IPomodoroSessionRepository repository,
        IInterruptSessionPresenter presenter,
        ICreateJournalEntryUseCase journalUseCase)
    {
        _repository = repository;
        _presenter = presenter;
        _journalUseCase = journalUseCase;
    }

    public async Task Execute(InterruptSessionRequest request, CancellationToken cancellationToken = default)
    {
        var session = await _repository.GetActiveAsync(cancellationToken);
        if (session is null)
        {
            _presenter.PresentFailure(PomodoroErrors.Pomodoro.SessionNotActive);
            return;
        }

        session.Interrupt(DateTime.UtcNow);
        await _repository.UpdateAsync(session, cancellationToken);

        await _journalUseCase.Execute(new CreateJournalEntryRequest(
            JournalEventType.SprintInterrupted,
            session.Id.Value,
            session.EndedAt!.Value), cancellationToken);

        _presenter.PresentSuccess();
    }
}
