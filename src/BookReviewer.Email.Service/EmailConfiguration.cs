namespace BookReviewer.Email.Service;

public class EmailConfiguration
{
    public required string Host { get; init; }
    public required int Port { get; init; }
    public required string Email { get; init; }
    public required string Password { get; init; }
    public required string FromName { get; init; }
    public required string FromEmail { get; init; }
}