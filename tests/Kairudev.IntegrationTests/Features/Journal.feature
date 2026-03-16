Feature: Journal Management
  As a developer
  I want to maintain a journal of my work
  So that I can track my daily progress

  Scenario: Create a journal entry
    Given I have a fresh database
    When I create a journal entry with event type "TaskCompleted" for resource "task-123"
    Then the entry should be created successfully
    And the entry should have a timestamp
    And the entry should have a sequence number

  Scenario: Add comment to journal entry
    Given I have a journal entry
    When I add a comment "Great progress today!"
    Then the comment should be linked to the entry
    And the entry should have 1 comment

  Scenario: Retrieve journal entries
    Given I have the following journal entries:
      | EventType | Count |
      | TaskCompleted | 3 |
      | SessionCompleted | 2 |
    When I retrieve all journal entries
    Then I should have 5 entries in chronological order
