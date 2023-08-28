using BookReviewer.Shared.Entities;
using BookReviewer.Shared.Repositories;
using MassTransit;
using BookReviewer.Shared.MassTransit.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

[ApiController]
[Route("reviews")]
public class ReviewsController : ControllerBase
{
    private readonly IRepository<Review> repository;
    private readonly IPublishEndpoint publishEndpoint;

    public ReviewsController(IRepository<Review> repository, IPublishEndpoint publishEndpoint)
    {
        this.repository = repository;
        this.publishEndpoint = publishEndpoint;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllReviewsByBookId([FromQuery] Guid bookId)
    {
        var books = await repository.GetAsync(x => x.BookId == bookId);
        return Ok(books);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateReview(CreateReviewDTO createReviewDTO)
    {
        var review = new Review
        {
            BookId = createReviewDTO.BookId,
            UserId = Guid.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value),
            Rating = createReviewDTO.Rating,
            Text = createReviewDTO.Text
        };

        await repository.CreateAsync(review);

        await publishEndpoint.Publish(new ReviewCreated
        {
            BookId = review.BookId,
            UserId = review.UserId,
            Rating = review.Rating,
            Text = review.Text
        });

        return NoContent();
    }

    [Authorize]
    [HttpPut]
    public async Task<IActionResult> UpdateReview(UpdateReviewDTO updateReviewDTO)
    {
        var review = new Review
        {
            BookId = updateReviewDTO.BookId,
            Rating = updateReviewDTO.Rating,
            Text = updateReviewDTO.Text
        };

        await repository.UpdateAsync(review);

        await publishEndpoint.Publish(new ReviewUpdated
        {
            BookId = review.BookId,
            Rating = review.Rating,
            Text = review.Text
        });

        return NoContent();
    }


    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteReview(Guid id)
    {
        await repository.DeleteAsync(id);

        await publishEndpoint.Publish(new ReviewDeleted { ReviewId = id });
        
        return NoContent();
    }
}