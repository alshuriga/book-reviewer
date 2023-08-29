namespace BookReviewer.Shared.Entities;

public class Review : Entity
{
    public required Guid BookId { get; set; }
    public required Guid UserId { get; set; }
    public required short Rating { get; set; }
    public required string Text { get; set; }
}
