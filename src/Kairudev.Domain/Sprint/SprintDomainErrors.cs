namespace Kairudev.Domain.Sprint;

public static class SprintDomainErrors
{
    public static class Sprint
    {
        public const string NameTooLong = "Sprint name cannot exceed 200 characters.";
        public const string EndedAtBeforeStartedAt = "EndedAt must be after StartedAt.";
        public const string NotFound = "Sprint session not found.";
    }
}
