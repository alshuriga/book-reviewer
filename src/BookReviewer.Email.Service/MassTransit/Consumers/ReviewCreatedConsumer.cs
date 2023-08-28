using BookReviewer.Email.Service.Repositories;
using BookReviewer.Shared.MassTransit.Contracts;
using MassTransit;

namespace BookReviewer.Email.Service.Masstransit.Consumers;

public class ReviewCreatedConsumer : IConsumer<ReviewCreated>
{
    private readonly IEmailSendingService emailSendingService;
    private readonly IEmailSubscribersRepository emailSubscribersRepository;

    public ReviewCreatedConsumer(IEmailSendingService emailSendingService, IEmailSubscribersRepository emailSubscribersRepository) 
    {
        this.emailSendingService = emailSendingService;
        this.emailSubscribersRepository = emailSubscribersRepository;
    }
    public async Task Consume(ConsumeContext<ReviewCreated> context)
    {
        var emails = await emailSubscribersRepository.GetEmailsForBookAsync(context.Message.BookId);
        await emailSendingService.SendEmailAsync(emails, $"New review just appeared! - book {context.Message.BookId}", $"a user just posted a review:\nRating: {context.Message.Rating}\n{context.Message.Text}");
        
    }
}

public class ReviewCreatedConsumerEmailDefinition : ConsumerDefinition<ReviewCreatedConsumer>
{
    public ReviewCreatedConsumerEmailDefinition()
    {
        EndpointName = "ReviewCreated-EmailNotifications";
    }
}
