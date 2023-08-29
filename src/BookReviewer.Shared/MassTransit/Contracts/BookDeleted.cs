namespace BookReviewer.Shared.MassTransit.Contracts;

public record BookDeleted 
{
    public required Guid BookId { get; init; }
}