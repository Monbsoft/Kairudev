Feature: User Management
  As a system
  I want to manage user accounts and settings
  So that users can personalize their experience

  Scenario: Create a new user
    Given I have a fresh database
    When I create a user with GitHub ID "github-123" and login "john-dev"
    Then the user should be created successfully
    And the user should have display name "john-dev"

  Scenario: Configure user settings
    Given I have a user
    When I update user settings with theme "dark" and ringtone "chime"
    Then the settings should be saved
    And the theme should be "dark"
    And the ringtone should be "chime"

  Scenario: Configure Jira integration
    Given I have a user
    When I configure Jira settings with URL and API token
    Then the Jira settings should be saved
    And the user should be able to access Jira integration
