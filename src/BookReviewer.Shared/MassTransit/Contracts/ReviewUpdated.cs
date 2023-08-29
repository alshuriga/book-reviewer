namespace BookReviewer.Shared.MassTransit.Contracts;

public record ReviewUpdated 
{
    public required Guid ReviewId { get; init; }
    public required Guid BookId { get; init; }
    public required Guid UserId { get; init; }
    public required short Rating { get; init; }
    public required string Text { get; init; }
}