namespace BookReviewer.Shared.MassTransit.Contracts;

public record BookCreated 
{
    public required Guid BookId { get; init; }
    public required string Title { get; init; }
    public required string Author { get; init; }
}