using BookReviewer.Shared.Entities;

namespace BookReviewer.Email.Service.Entities;

public class ReviewSubscriptionData : Entity
{
    public required Guid BookId { get; set; }
    public required List<string> Emails { get; set; }
}