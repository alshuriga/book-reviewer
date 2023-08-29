using Amazon.Util;
using BookReviewer.Email.Service.Entities;
using BookReviewer.Shared.MassTransit.Contracts;
using BookReviewer.Shared.Repositories;
using MassTransit;
using MimeKit;

namespace BookReviewer.Email.Service.Masstransit.Consumers;

public class BookCreatedConsumer : IConsumer<BookCreated>
{
    private readonly IRepository<EmailBookInfo> repository;

    public BookCreatedConsumer(IRepository<EmailBookInfo> repository)
    {
        this.repository = repository;
    }
    public async Task Consume(ConsumeContext<BookCreated> context)
    {
        var entity = new EmailBookInfo() {
            Id = context.Message.BookId,
            Title = context.Message.Title,
            Author = context.Message.Author
        };

        await repository.CreateAsync(entity);
    }
}
