using BookReviewer.Shared.Entities;
using BookReviewer.Shared.MassTransit.Contracts;
using BookReviewer.Shared.Repositories;
using MassTransit;

namespace BookReviewer.Shared.MassTransit.Consumers;

public class ReviewUpdatedConsumer : IConsumer<ReviewUpdated>
{
    private readonly IRepository<Review> repository;

    public ReviewUpdatedConsumer(IRepository<Review> repository)
    {
        this.repository = repository;
    }
    public async Task Consume(ConsumeContext<ReviewUpdated> context)
    {
        var reviewEntity = new Review 
        {
            Id = context.Message.ReviewId,
            BookId = context.Message.BookId,
            UserId = context.Message.UserId,
            Rating = context.Message.Rating,
            Text = context.Message.Text
        };

        await repository.CreateAsync(reviewEntity);
    }
}
