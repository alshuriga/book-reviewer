namespace BookReviewer.Shared.Auth;

public class JwtSettings 
{
    public string JwtSecret { get; init; }
    public string JwtIssuer { get; init; }
    public int LifetimeSeconds { get; init; }
}