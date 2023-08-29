namespace BookReviewer.Shared.Entities;

public class Book : Entity
{
    public required string Title { get; set; }
    public required string Author { get; set; }
    public required string Description {get; set;}
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}

