namespace BookReviewer.Email.Service;

public class EmailConfiguration
{
    public string Host { get; init; }
    public int Port { get; init; }
    public string Email { get; init; }
    public string Password { get; init; }
    public string FromName { get; init; }
    public string FromEmail { get; init; }
}