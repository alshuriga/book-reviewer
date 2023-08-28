namespace BookReviewer.Shared.Entities;

public class Review : Entity
{
    public Guid BookId { get; set; }
    public Guid UserId { get; set; }
    public short Rating { get; set; }
    public string Text { get; set; }
}
