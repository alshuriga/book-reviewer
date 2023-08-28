namespace BookReviewer.Shared.MassTransit.Contracts;

public record ReviewCreated
{
    public Guid ReviewId { get; init; }
    public Guid BookId { get; init; }
    public Guid UserId { get; init; }
    public short Rating { get; init; }
    public string Text { get; init; }
}