using BookReviewer.Books.Service.DTO;
using BookReviewer.Shared.Entities;
using BookReviewer.Shared.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookReviewer.Books.Service.Controllers;

[ApiController]
[Route("books/")]

public class BooksController : ControllerBase
{
    private readonly IRepository<Book> booksRepository;
    private readonly IRepository<Review> reviewsRepository;
    public BooksController(IRepository<Book> booksRepository, IRepository<Review> reviewsRepository) 
    {
        this.booksRepository = booksRepository;
        this.reviewsRepository = reviewsRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllBooks()
    {
        var books = (await booksRepository.GetAsync()).Select(async b => new BookDTO(
            b.Id,
            b.Title,
            b.Author,
            b.Description,
            (await reviewsRepository.GetAsync(r => r.BookId == b.Id))
                .Select(r => new ReviewDTO(r.UserId, r.Rating, r.Text)).ToArray()));
                
        return Ok(books);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateBook(CreateBookDTO createBookDTO)
    {
        var book = new Book {
            Title = createBookDTO.Title,
            Author = createBookDTO.Author,
            Description = createBookDTO.Description
        };

        await booksRepository.CreateAsync(book);
        return NoContent();
    }

    [Authorize]
    [HttpPut]
    public async Task<IActionResult> UpdateBook(UpdateBookDTO updateBookDTO)
    {
        var book = new Book {
            Id = updateBookDTO.Id,
            Title = updateBookDTO.Title,
            Author = updateBookDTO.Author,
            Description = updateBookDTO.Description
        };

        await booksRepository.UpdateAsync(book);
        return NoContent();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBook(Guid id)
    {
        await booksRepository.DeleteAsync(id);
        return NoContent();
    }
}