namespace BookReviewer.Shared.Auth;

public class JwtSettings 
{
    public string JwtSecret { get; init; }  = null!;
    public string JwtIssuer { get; init; } = null!;
    public int LifetimeSeconds { get; init; }
}