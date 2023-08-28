namespace BookReviewer.Email.Service;

public class EmailConfiguration
{
    public string Host { get; init; }  = null!;
    public int Port { get; init; }
    public string Email { get; init; } = null!;
    public string Password { get; init; } = null!;
    public string FromName { get; init; } = null!;
    public string FromEmail { get; init; } = null!;
}