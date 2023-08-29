namespace BookReviewer.Shared.MassTransit.Contracts;

public record ReviewDeleted
{
    public required Guid ReviewId { get; init; }
}