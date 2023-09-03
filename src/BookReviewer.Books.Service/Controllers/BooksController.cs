using BookReviewer.Books.Service.DTO;
using BookReviewer.Shared.Entities;
using BookReviewer.Shared.MassTransit.Contracts;
using BookReviewer.Shared.Repositories;
using FluentValidation;
using MassTransit;
using MassTransit.Scheduling;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Swashbuckle.AspNetCore.Annotations;

namespace BookReviewer.Books.Service.Controllers;

[Route("books")]
[ApiController]
public class BooksController : ControllerBase
{
    private readonly IRepository<Book> booksRepository;
    private readonly IRepository<Review> reviewsRepository;

    private readonly IPublishEndpoint publishEndpoint;
    public BooksController(IRepository<Book> booksRepository, IRepository<Review> reviewsRepository, IPublishEndpoint publishEndpoint) 
    {
        this.booksRepository = booksRepository;
        this.reviewsRepository = reviewsRepository;
        this.publishEndpoint = publishEndpoint;
    }

    [SwaggerOperation("Get a list of all books")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet]
    public async Task<IActionResult> GetAllBooks()
    {
        var books = new List<BookDTO>();
        foreach(var book in await booksRepository.GetAsync())
        {
            var reviews = await reviewsRepository.GetAsync(r => r.BookId == book.Id);
            var dto = new BookDTO(book.Id, book.Title, book.Author, book.Description, reviews.Select(r => new ReviewDTO(r.UserId, r.Rating, r.Text)).ToArray());
            books.Add(dto);
        }

        return Ok(books);
    }

    [SwaggerOperation("Create a book")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> CreateBook(CreateBookDTO createBookDTO)
    {
        var book = new Book {
            Title = createBookDTO.Title,
            Author = createBookDTO.Author,
            Description = createBookDTO.Description
        };

        await booksRepository.CreateAsync(book);

        await publishEndpoint.Publish(new BookCreated
        {
            BookId = book.Id,
            Title = book.Title,
            Author = book.Author
        });
        
        return NoContent();
    }

    [SwaggerOperation("Update a book")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [Authorize(Roles = "Admin")]
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

        await publishEndpoint.Publish(new BookUpdated
        {
            BookId = book.Id,
            Title = book.Title,
            Author = book.Author
        });

        return NoContent();
    }

    [SwaggerOperation("Delete a book")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBook(Guid id)
    {
        await booksRepository.DeleteAsync(id);

        await publishEndpoint.Publish(new BookDeleted() { BookId = id });

        return NoContent();
    }
}