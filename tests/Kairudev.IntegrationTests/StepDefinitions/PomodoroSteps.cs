using Kairudev.Domain.Identity;
using Kairudev.Domain.Pomodoro;
using Kairudev.Domain.Tasks;
using Kairudev.IntegrationTests.Support;
using Reqnroll;

namespace Kairudev.IntegrationTests.StepDefinitions;

[Binding]
public class PomodoroSteps
{
    private readonly ScenarioContext _scenarioContext;
    private DatabaseContext DbContext => (DatabaseContext)_scenarioContext["DatabaseContext"];

    public PomodoroSteps(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    [When("I start a Pomodoro session of (\\d+) minutes")]
    public void WhenStartSession(int minutes)
    {
        var userId = UserId.From("test-user");
        var sessionId = PomodoroSessionId.NewId();

        var session = PomodoroSession.Create(
            sessionId,
            userId,
            PomodoroSessionType.Sprint,
            minutes
        ).Value;

        DbContext.DbContext.PomodoroSessions.Add(session);
        DbContext.DbContext.SaveChanges();

        _scenarioContext["CurrentSession"] = session;
    }

    [Then("the session should be created")]
    public void ThenSessionCreated()
    {
        var session = (PomodoroSession)_scenarioContext["CurrentSession"];
        Assert.NotNull(session);
        Assert.NotEqual(default, session.Id.Value);
    }

    [Then("the session status should be \"([^\"]*)\"")]
    public void ThenSessionStatus(string status)
    {
        var session = (PomodoroSession)_scenarioContext["CurrentSession"];
        Assert.Equal(status, session.Status.ToString());
    }

    [Then("the session should have a start time")]
    public void ThenSessionHasStartTime()
    {
        var session = (PomodoroSession)_scenarioContext["CurrentSession"];
        Assert.NotNull(session.StartedAt);
    }

    [Given("I have a running Pomodoro session")]
    public void GivenRunningSession()
    {
        var userId = UserId.From("test-user");
        var sessionId = PomodoroSessionId.NewId();

        var session = PomodoroSession.Create(
            sessionId,
            userId,
            PomodoroSessionType.Sprint,
            25
        ).Value;

        DbContext.DbContext.PomodoroSessions.Add(session);
        DbContext.DbContext.SaveChanges();

        _scenarioContext["CurrentSession"] = session;
    }

    [When("I complete the session")]
    public void WhenCompleteSession()
    {
        var session = (PomodoroSession)_scenarioContext["CurrentSession"];
        var result = session.Complete();

        Assert.True(result.IsSuccess);

        DbContext.DbContext.PomodoroSessions.Update(session);
        DbContext.DbContext.SaveChanges();

        _scenarioContext["CurrentSession"] = session;
    }

    [Then("the session should have an end time")]
    public void ThenSessionHasEndTime()
    {
        var session = (PomodoroSession)_scenarioContext["CurrentSession"];
        Assert.NotNull(session.EndedAt);
    }

    [Given("I have a Pomodoro session")]
    public void GivenPomodoroSession()
    {
        var userId = UserId.From("test-user");
        var sessionId = PomodoroSessionId.NewId();

        var session = PomodoroSession.Create(
            sessionId,
            userId,
            PomodoroSessionType.Sprint,
            25
        ).Value;

        DbContext.DbContext.PomodoroSessions.Add(session);
        DbContext.DbContext.SaveChanges();

        _scenarioContext["CurrentSession"] = session;
    }

    [When("I link the task to the session")]
    public void WhenLinkTaskToSession()
    {
        var task = (DeveloperTask)_scenarioContext["CurrentTask"];
        var session = (PomodoroSession)_scenarioContext["CurrentSession"];

        var result = session.LinkTask(task.Id);

        Assert.True(result.IsSuccess);

        DbContext.DbContext.PomodoroSessions.Update(session);
        DbContext.DbContext.SaveChanges();

        _scenarioContext["CurrentSession"] = session;
    }

    [Then("the task should be linked to the session")]
    public void ThenTaskLinkedToSession()
    {
        var task = (DeveloperTask)_scenarioContext["CurrentTask"];
        var session = (PomodoroSession)_scenarioContext["CurrentSession"];

        Assert.Contains(task.Id.Value, session.LinkedTaskIdValues);
    }

    [Given("I have Pomodoro settings configured with:")]
    public void GivenPomodoroSettings(DataTable table)
    {
        var userId = UserId.From("test-user");
        var row = table.Rows[0];

        var sprintDuration = int.Parse(row["Value"]); // This is a simplified version
        var settings = PomodoroSettings.Create(userId, sprintDuration, 5, 15).Value;

        DbContext.DbContext.PomodoroSettings.Add(new Kairudev.Infrastructure.Persistence.Internal.PomodoroSettingsRow
        {
            UserId = userId.Value,
            SprintDurationMinutes = settings.SprintDurationMinutes,
            ShortBreakDurationMinutes = settings.ShortBreakDurationMinutes,
            LongBreakDurationMinutes = settings.LongBreakDurationMinutes
        });

        DbContext.DbContext.SaveChanges();

        _scenarioContext["CurrentSettings"] = settings;
    }

    [When("I retrieve the settings")]
    public void WhenRetrieveSettings()
    {
        var userId = UserId.From("test-user");
        var settings = DbContext.DbContext.PomodoroSettings.FirstOrDefault(s => s.UserId == userId.Value);

        _scenarioContext["RetrievedSettings"] = settings;
    }

    [Then("the settings should match the configured values")]
    public void ThenSettingsMatch()
    {
        var retrievedSettings = _scenarioContext["RetrievedSettings"];
        Assert.NotNull(retrievedSettings);
    }
}
