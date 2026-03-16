Feature: Task Management
  As a developer
  I want to manage my daily tasks
  So that I can organize my work

  Scenario: Create a new task
    Given I have a fresh database
    When I create a task with title "Implement API" and description "Build REST endpoints"
    Then the task should be created successfully
    And the task status should be "Todo"
    And the task should have a creation date

  Scenario: Update task status
    Given I have a task "Fix bug" with status "Todo"
    When I complete the task
    Then the task status should be "Done"
    And the task should have a completion date

  Scenario: Link task to Jira ticket
    Given I have a task "Implement feature"
    When I link it to Jira ticket "PROJ-123"
    Then the task should be linked to Jira ticket "PROJ-123"

  Scenario: Retrieve all tasks for user
    Given I have the following tasks:
      | Title          | Status |
      | Task 1         | Todo   |
      | Task 2         | In Progress |
      | Task 3         | Done   |
    When I retrieve all tasks
    Then I should have 3 tasks
    And the tasks should be in the correct order
