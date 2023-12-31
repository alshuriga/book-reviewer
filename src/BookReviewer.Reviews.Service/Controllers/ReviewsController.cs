using BookReviewer.Shared.Entities;
using BookReviewer.Shared.Repositories;
using MassTransit;
using BookReviewer.Shared.MassTransit.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using BookReviewer.Reviews.Service.DTO;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BookReviewer.Reviews.Controllers;

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

    [SwaggerOperation("Get a list of reviews of specified book")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet]
    public async Task<IActionResult> GetAllReviewsByBookId([FromQuery] Guid bookId)
    {
        var books = await repository.GetAsync(x => x.BookId == bookId);
        return Ok(books);
    }

    [SwaggerOperation("Create a review")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateReview(CreateReviewDTO createReviewDTO)
    {
        var review = new Review
        {
            BookId = createReviewDTO.BookId,
            UserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!),
            Rating = createReviewDTO.Rating,
            Text = createReviewDTO.Text
        };

        await repository.CreateAsync(review);

        await publishEndpoint.Publish(new ReviewCreated
        {
            ReviewId = review.Id,
            BookId = review.BookId,
            UserId = review.UserId,
            Rating = review.Rating,
            Text = review.Text
        });

        return NoContent();
    }

    [SwaggerOperation("Update a review")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize]
    [HttpPut]
    public async Task<IActionResult> UpdateReview(UpdateReviewDTO updateReviewDTO)
    {
        var review = await repository.GetByIdAsync(updateReviewDTO.ReviewId);

        if(review == null)
            return NotFound();

        if(review.UserId.ToString() != User.FindFirstValue(ClaimTypes.NameIdentifier))
            return Forbid();

        review.Rating = updateReviewDTO.Rating;
        review.Text = updateReviewDTO.Text;

        await repository.UpdateAsync(review);

        await publishEndpoint.Publish(new ReviewUpdated
        {
            ReviewId = review.Id,
            UserId = review.UserId,
            BookId = review.BookId,
            Rating = review.Rating,
            Text = review.Text
        });

        return NoContent();
    }

    [SwaggerOperation("Delete a review")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteReview(Guid id)
    {   
        var review = await repository.GetByIdAsync(id);

        if(review == null) return NotFound();

        if(review.UserId.ToString() != User.FindFirstValue(ClaimTypes.NameIdentifier)) return Forbid();

        await repository.DeleteAsync(id);
        await publishEndpoint.Publish(new ReviewDeleted { ReviewId = id });
        
        return NoContent();
    }
}