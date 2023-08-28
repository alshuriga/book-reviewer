namespace BookReviewer.Shared.MassTransit.Contracts;

public record ReviewDeleted
{
    public Guid ReviewId { get; init; }
}