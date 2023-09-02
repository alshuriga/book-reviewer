using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

using BookReviewer.IdentityProvider.IdentityModels;
using BookReviewer.Shared.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BookReviewer.IdentityProvider.Services;

public class JwtProvider
{
    private readonly JwtSettings jwtSettings;

    private readonly UserManager<ApplicationUser> userManager;
    public JwtProvider(IOptions<JwtSettings> jwtSettings, UserManager<ApplicationUser> userManager)
    {
        this.jwtSettings = jwtSettings.Value;
        this.userManager = userManager;
    }

    public async Task<string> GenerateJWT(ApplicationUser user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.JwtSecret));

        var claims = new List<Claim>() { 
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email!),
                };

        foreach(var role in await userManager.GetRolesAsync(user))
        {
            if(role != null)
                claims.Add(new Claim(ClaimTypes.Role, role));
        }
                
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