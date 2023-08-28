namespace BookReviewer.Shared.Entities;

public class Book : Entity
{
    public string Title { get; set; } = null!;
    public string Author { get; set; } = null!;
    public string Description {get; set;} = null!;
    public IList<Review> Reviews { get; set; } = null!;
}

