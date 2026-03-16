Feature: Pomodoro Sessions
  As a developer
  I want to track my work sessions with Pomodoro technique
  So that I can measure my productivity

  Scenario: Start a new Pomodoro session
    Given I have a fresh database
    When I start a Pomodoro session of 25 minutes
    Then the session should be created
    And the session status should be "Running"
    And the session should have a start time

  Scenario: Complete a Pomodoro session
    Given I have a running Pomodoro session
    When I complete the session
    Then the session status should be "Completed"
    And the session should have an end time

  Scenario: Link task to Pomodoro session
    Given I have a task "Implement feature"
    And I have a Pomodoro session
    When I link the task to the session
    Then the task should be linked to the session

  Scenario: Get Pomodoro settings
    Given I have Pomodoro settings configured with:
      | Setting | Value |
      | Sprint Duration | 25 |
      | Short Break | 5 |
      | Long Break | 15 |
    When I retrieve the settings
    Then the settings should match the configured values
