namespace Kairudev.Web.Services;

public sealed record TaskDto(
    Guid Id,
    string Title,
    string Status,
    DateTime CreatedAt,
    DateTime? CompletedAt);
