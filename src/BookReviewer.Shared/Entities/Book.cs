namespace BookReviewer.Shared.Entities;

public class Book : Entity
{
    public string Title { get; set; }
    public string Author { get; set; }
    public string Description {get; set;}
    public IList<Review> Reviews { get; set; }
}

