using Reqnroll;

namespace Kairudev.IntegrationTests.Support;

[Binding]
public class Hooks
{
    private readonly ScenarioContext _scenarioContext;

    public Hooks(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    [BeforeScenario]
    public void BeforeScenario()
    {
        var dbContext = new DatabaseContext();
        _scenarioContext["DatabaseContext"] = dbContext;
    }

    [AfterScenario]
    public void AfterScenario()
    {
        if (_scenarioContext.TryGetValue("DatabaseContext", out DatabaseContext? dbContext))
        {
            dbContext?.Dispose();
        }
    }
}
