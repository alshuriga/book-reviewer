using BookReviewer.Email.Service.Entities;
using BookReviewer.Shared.Repositories;
using MailKit.Net.Smtp;
using MimeKit;

namespace BookReviewer.Email.Service;

public class EmailSendingService : IEmailSendingService
{
    private readonly EmailConfiguration emailConfig;
    public EmailSendingService(EmailConfiguration emailConfig)
    {
        this.emailConfig = emailConfig;
        
    }
    public async Task SendEmailAsync(IEnumerable<string> emails, string subject, string content)
    {
        var message = new MimeMessage();
        
        message.From.Add(new MailboxAddress(emailConfig.FromName, emailConfig.FromEmail));

        foreach(var email in emails)
            message.To.Add(new MailboxAddress(email, email));

        message.Subject = subject;

        message.Body = new TextPart("plain") { Text = content };

        using var client = new SmtpClient();
        
        await client.ConnectAsync(emailConfig.Host, emailConfig.Port, false);

        await client.AuthenticateAsync(emailConfig.Email, emailConfig.Password);

        var result = await client.SendAsync(message);

        await client.DisconnectAsync(true);
    }
}