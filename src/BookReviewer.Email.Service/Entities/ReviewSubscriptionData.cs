using BookReviewer.Shared.Entities;

namespace BookReviewer.Email.Service.Entites;

public class ReviewSubscriptionData : Entity
{
    public required Guid ReviewId { get; set; }
    public required List<string> Emails { get; set; }
}