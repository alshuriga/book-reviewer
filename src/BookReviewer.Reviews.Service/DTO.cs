using System.ComponentModel.DataAnnotations;

namespace BookReviewer.Reviews.Service;

public record ReviewDTO(Guid UserId, short Rating, string Text);

public record CreateReviewDTO(
    Guid BookId,
    [Range(1,10, ErrorMessage = "Rating must be between 1 and 10")] short Rating,
    [StringLength(500, MinimumLength = 1)] string Text
    );

public record UpdateReviewDTO(
    Guid ReviewId,
    Guid BookId,
    [Range(1,10, ErrorMessage = "Rating must be between 1 and 10")] short Rating,
    [StringLength(500, MinimumLength = 1)] string Text
    );