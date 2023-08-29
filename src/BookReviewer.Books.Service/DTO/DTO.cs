using System.ComponentModel.DataAnnotations;

namespace BookReviewer.Books.Service.DTO;

public record BookDTO(Guid BookId, string Title, string Author, string Description, ReviewDTO[] Reviews);

public record ReviewDTO(
    Guid UserId,
    short Rating,
    string Text);

public record CreateBookDTO(
    [StringLength(50, MinimumLength = 1)] string Title,
    [StringLength(50, MinimumLength = 1)] string Author,
    [StringLength(500, MinimumLength = 1)] string Description
    );

public record UpdateBookDTO(
    Guid Id,
    [StringLength(50, MinimumLength = 1)] string Title,
    [StringLength(50, MinimumLength = 1)] string Author,
    [StringLength(500, MinimumLength = 1)] string Description
);


