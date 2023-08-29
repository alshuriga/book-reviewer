using BookReviewer.Shared.Entities;

namespace BookReviewer.Email.Service.Entities;

public class EmailBookInfo : Entity
{
    public required string Title { get; set; }
    public required string Author { get; set; }
}