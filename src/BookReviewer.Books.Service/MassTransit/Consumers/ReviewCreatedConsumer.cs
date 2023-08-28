using BookReviewer.Shared.Entities;
using BookReviewer.Shared.MassTransit.Contracts;
using BookReviewer.Shared.Repositories;
using MassTransit;

namespace BookReviewer.Shared.MassTransit.Consumers;

public class ReviewCreatedConsumer : IConsumer<ReviewCreated>
{
    private readonly IRepository<Review> repository;

    public ReviewCreatedConsumer(IRepository<Review> repository)
    {
        this.repository = repository;
    }
    public async Task Consume(ConsumeContext<ReviewCreated> context)
    {
        var reviewEntity = new Review 
        {
            BookId = context.Message.BookId,
            UserId = context.Message.UserId,
            Rating = context.Message.Rating,
            Text = context.Message.Text
        };

        await repository.CreateAsync(reviewEntity);
    }
}
