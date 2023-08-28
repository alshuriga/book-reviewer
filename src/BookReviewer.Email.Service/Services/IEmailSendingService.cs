namespace BookReviewer.Email.Service;

public interface IEmailSendingService
{
    Task SendEmailAsync(IEnumerable<string> emails, string subject, string content);
}