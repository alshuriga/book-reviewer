using Amazon.Util;
using BookReviewer.Email.Service.Entities;
using BookReviewer.Shared.MassTransit.Contracts;
using BookReviewer.Shared.Repositories;
using MassTransit;
using MimeKit;

namespace BookReviewer.Email.Service.Masstransit.Consumers;

public class BookUpdatedConsumer : IConsumer<BookUpdated>
{
    private readonly IRepository<EmailBookInfo> repository;

    public BookUpdatedConsumer(IRepository<EmailBookInfo> repository)
    {
        this.repository = repository;
    }
    public async Task Consume(ConsumeContext<BookUpdated> context)
    {
        var existingEntity = repository.GetByIdAsync(context.Message.BookId);
        
        var entity = new EmailBookInfo() 
        {
            Id = context.Message.BookId,
            Title = context.Message.Title,
            Author = context.Message.Author
        };

        if (existingEntity == null) 
        {
            await repository.CreateAsync(entity);

            return;
        }

        await repository.UpdateAsync(entity);
    }
}
