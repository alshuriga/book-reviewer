using Amazon.Util;
using BookReviewer.Email.Service.Entities;
using BookReviewer.Shared.MassTransit.Contracts;
using BookReviewer.Shared.Repositories;
using MassTransit;
using MimeKit;

namespace BookReviewer.Email.Service.Masstransit.Consumers;

public class BookDeletedConsumer : IConsumer<BookDeleted>
{
    private readonly IRepository<EmailBookInfo> repository;

    public BookDeletedConsumer(IRepository<EmailBookInfo> repository)
    {
        this.repository = repository;
    }
    public async Task Consume(ConsumeContext<BookDeleted> context)
    {
        await repository.DeleteAsync(context.Message.BookId);
    }
}
