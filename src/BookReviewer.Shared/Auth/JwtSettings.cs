namespace BookReviewer.Shared.Auth;

public class JwtSettings 
{
    public required string JwtSecret { get; init; }
    public required string JwtIssuer { get; init; }
    public required int LifetimeSeconds { get; init; }
}