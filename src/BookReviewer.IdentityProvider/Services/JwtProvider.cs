using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using BookReviewer.InentityProvider.IdentityModels;
using BookReviewer.Shared.Auth;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BookReviewer.InentityProvider.Services;

public class JwtProvider
{
    private readonly JwtSettings jwtSettings;
    public JwtProvider(IOptions<JwtSettings> jwtSettings)
    {
        this.jwtSettings = jwtSettings.Value;
    }

    public string GenerateJWT(ApplicationUser user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.JwtSecret));

        var claims = new List<Claim>() { 
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email!)
                };

        foreach(var role in user.Roles) claims.Add(new Claim(ClaimTypes.Role, role));

        var descriptor = new JwtSecurityToken(
            claims: claims,
            signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256),
            expires: DateTime.Now.AddSeconds(jwtSettings.LifetimeSeconds),
            issuer: jwtSettings.JwtIssuer
        );

        var token = new JwtSecurityTokenHandler().WriteToken(descriptor);

        return token;
    }
}