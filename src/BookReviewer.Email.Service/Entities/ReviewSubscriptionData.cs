using BookReviewer.Shared.Entities;

namespace BookReviewer.Email.Service.Entites;

public class ReviewSubscriptionData : Entity
{
    public Guid ReviewId { get; set; }
    public List<string> Emails { get; set; }
}