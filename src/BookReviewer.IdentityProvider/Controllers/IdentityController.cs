using BookReviewer.InentityProvider.DTOs;
using BookReviewer.InentityProvider.IdentityModels;
using BookReviewer.InentityProvider.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookReviewer.InentityProvider.Controller;

[Route("users")]
[ApiController]
public class IdentityController : ControllerBase
{
    private readonly UserManager<ApplicationUser> userManager;

    private readonly JwtProvider jwtProvider;
    public IdentityController(UserManager<ApplicationUser> userManager, JwtProvider jwtProvider)
    {
        this.userManager = userManager;
        this.jwtProvider = jwtProvider;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateUserDTO userDTO)
    {
        var result = await userManager.CreateAsync(new ApplicationUser { UserName = userDTO.Email, Email = userDTO.Email }, userDTO.Password);
        if (!result.Succeeded) return BadRequest(result.Errors);
        return NoContent();
    }

    [HttpGet]
    public IActionResult GetAllUsers()
    {
        var users = userManager.Users.Select(u => new UserDTO(u.Id, u.Email, u.Roles));
        return Ok(users);
    }

    [HttpPost("signin")]
    public async Task<IActionResult> SignIn(SignInUserDTO signInUserDTO)
    {
        var user = userManager.Users.FirstOrDefault(u => u.Email == signInUserDTO.Email);
        if (user != null)
        {
            var res = await userManager.CheckPasswordAsync(user, signInUserDTO.Password);
            if (res)
            {
                var roles = (await userManager.GetRolesAsync(user)).ToArray();
                return Ok(new { token = jwtProvider.GenerateJWT(user) });
            }

        }
        return Unauthorized();
    }
}