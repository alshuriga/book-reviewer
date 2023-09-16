namespace BookReviewer.Email.DTO;

public record SubscribeUserToBookDTO(Guid BookId);

public record UserSubscriptionsDTO(ICollection<Guid> Guids);