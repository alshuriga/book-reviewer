using System.ComponentModel.DataAnnotations;

namespace BookReviewer.Books.Service.DTO;

public record BookDTO(Guid BookId, string Title, string Author, string Description, ReviewDTO[] Reviews);

public record ReviewDTO(Guid UserId,short Rating, string Text);

public record CreateBookDTO(string Title, string Author, string Description);

public record UpdateBookDTO(Guid Id, string Title, string Author, string Description);


