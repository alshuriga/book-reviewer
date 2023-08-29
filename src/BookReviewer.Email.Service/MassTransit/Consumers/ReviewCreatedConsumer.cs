using Amazon.Auth.AccessControlPolicy;
using BookReviewer.Email.Service.Entities;
using BookReviewer.Email.Service.Repositories;
using BookReviewer.Shared.MassTransit.Contracts;
using BookReviewer.Shared.Repositories;
using MassTransit;

namespace BookReviewer.Email.Service.Masstransit.Consumers;

public class ReviewCreatedConsumer : IConsumer<ReviewCreated>
{
    private readonly IRepository<EmailBookInfo> emailBookInfoRepository;
    private readonly IEmailSendingService emailSendingService;
    private readonly IEmailSubscribersRepository emailSubscribersRepository;

    public ReviewCreatedConsumer(IEmailSendingService emailSendingService, IEmailSubscribersRepository emailSubscribersRepository, IRepository<EmailBookInfo> emailBookInfoRepository)
    {
        this.emailSendingService = emailSendingService;
        this.emailSubscribersRepository = emailSubscribersRepository;
        this.emailBookInfoRepository = emailBookInfoRepository;
    }
    public async Task Consume(ConsumeContext<ReviewCreated> context)
    {
        var bookInfo = await emailBookInfoRepository.GetByIdAsync(context.Message.BookId) ?? throw new ArgumentException("Book not found");
        var emails = await emailSubscribersRepository.GetEmailsForBookAsync(context.Message.BookId);
        await emailSendingService.SendEmailAsync(emails, $"New review for \"{bookInfo.Title}\" by {bookInfo.Author}",  $"Some user just posted a review:\nRating: {context.Message.Rating}\n{context.Message.Text}");     
    }
}

public class ReviewCreatedConsumerEmailDefinition : ConsumerDefinition<ReviewCreatedConsumer>
{
    public ReviewCreatedConsumerEmailDefinition()
    {
        EndpointName = "ReviewCreated-EmailNotifications";
    }
}
