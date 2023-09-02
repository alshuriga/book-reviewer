using BookReviewer.IdentityProvider.DTO;
using BookReviewer.IdentityProvider.IdentityModels;
using BookReviewer.IdentityProvider.Services;
using MassTransit.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver.Linq;
using Swashbuckle.AspNetCore.Annotations;

namespace BookReviewer.IdentityProvider.Controller;

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


    [SwaggerOperation("Create a new user")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateUserDTO userDTO)
    {
        var result = await userManager.CreateAsync(new ApplicationUser { UserName = userDTO.Email, Email = userDTO.Email }, userDTO.Password);
        if (!result.Succeeded) return BadRequest(result.Errors);
        return NoContent();
    }


    [SwaggerOperation("Get a list of all users")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public IActionResult GetAllUsers()
    {
        var users = userManager.Users.Select(u => new UserDTO(u.Id, u.Email!, u.Roles));
        return Ok(users);
    }

    [SwaggerOperation("Assign admin permissions to a user")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [HttpPost("admin")]
    public async Task<IActionResult> AssignAdmin(UserRoleManagementDTO userRoleManagementDTO)
    {
        var user = userManager.Users.FirstOrDefault(u => u.Id == userRoleManagementDTO.UserId);

        if(user == null)
            return NotFound("User not found.");

        var res = await userManager.AddToRoleAsync(user, "Admin");

        if(!res.Succeeded)
            throw new ApplicationException("An error occured while assigning admin role");

        return NoContent();
    }

    [SwaggerOperation("Suspend admin permissions from a user")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [HttpDelete("admin")]
    public async Task<IActionResult> SuspendAdmin(UserRoleManagementDTO userRoleManagementDTO)
    {
        var user = userManager.Users.FirstOrDefault(u => u.Id == userRoleManagementDTO.UserId);

        if(user == null)
            return NotFound("User not found.");

        if(!user.Roles.Contains("Admin"))
            return BadRequest("User doesn't have admin rights.");

        if(userManager.Users.Where(u => u.Roles.Contains("Admin")).Count() <= 1)
            return BadRequest("There must be at least one user with admin rights.");

        var res = await userManager.RemoveFromRoleAsync(user, "Admin");

        if(!res.Succeeded)
            throw new ApplicationException("An error occured while assigning admin role.");

        return NoContent();
    }

    [SwaggerOperation("Generate JWT for authentication/authorization")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpPost("signin")]
    public async Task<IActionResult> SignIn(SignInUserDTO signInUserDTO)
    {
        var user = userManager.Users.FirstOrDefault(u => u.Email == signInUserDTO.Email);
        if (user == null) return NotFound("User not found.");
        var res = await userManager.CheckPasswordAsync(user, signInUserDTO.Password);
        if (res)
        {
            var roles = (await userManager.GetRolesAsync(user)).ToArray();
            return Ok(new { token = await jwtProvider.GenerateJWT(user) });
        }
        return Unauthorized();
        
    }
}