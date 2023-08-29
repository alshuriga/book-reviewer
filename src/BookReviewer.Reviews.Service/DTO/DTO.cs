namespace BookReviewer.Reviews.Service.DTO;

public record ReviewDTO(Guid UserId, short Rating, string Text);

public record CreateReviewDTO(Guid BookId, short Rating, string Text);

public record UpdateReviewDTO(Guid ReviewId, Guid BookId, short Rating, string Text);