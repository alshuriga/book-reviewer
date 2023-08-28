using BookReviewer.Shared.Entities;
using BookReviewer.Shared.MassTransit.Contracts;
using BookReviewer.Shared.Repositories;
using MassTransit;

namespace BookReviewer.Shared.MassTransit.Consumers;

public class ReviewDeletedConsumer : IConsumer<ReviewDeleted>
{
    private readonly IRepository<Review> repository;

    public ReviewDeletedConsumer(IRepository<Review> repository)
    {
        this.repository = repository;
    }
    public async Task Consume(ConsumeContext<ReviewDeleted> context)
    {
        await repository.DeleteAsync(context.Message.ReviewId);
    }
}
