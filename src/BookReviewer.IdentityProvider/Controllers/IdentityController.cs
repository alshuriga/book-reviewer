using BookReviewer.IdentityProvider.DTO;
using BookReviewer.IdentityProvider.IdentityModels;
using BookReviewer.IdentityProvider.Services;
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
    private readonly RoleManager<ApplicationRole> roleManager;

    private readonly JwtProvider jwtProvider;
    public IdentityController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, JwtProvider jwtProvider)
    {
        this.userManager = userManager;
        this.roleManager = roleManager;
        this.jwtProvider = jwtProvider;
    }


    [SwaggerOperation("Create a new user")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
        var allRoles = roleManager.Roles.ToList();
        var users = userManager.Users.ToList().Select(u => new UserDTO(
            u.Id,
            u.Email!,
            u.Roles.Select(i => {
                var id = Guid.Parse(i);
                return new RoleDTO(id, allRoles.First(r => r.Id == id).NormalizedName!);
        })));
        return Ok(users);
    }

    [SwaggerOperation("Assign admin permissions to a user")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPut("admin")]
    public async Task<IActionResult> SuspendAdmin(UserRoleManagementDTO userRoleManagementDTO)
    {
        var user = userManager.Users.FirstOrDefault(u => u.Id == userRoleManagementDTO.UserId);

        if(user == null)
            return NotFound("User not found.");

        var usersInRole = await userManager.GetUsersInRoleAsync("Admin");

        if(usersInRole.Count() <= 1)
            return BadRequest("There must be at least one user with admin rights.");

        var res = await userManager.RemoveFromRoleAsync(user, "Admin");

        if(!res.Succeeded)
            throw new ApplicationException("An error occured while assigning admin role.");

        return NoContent();
    }

    [SwaggerOperation("Generate JWT for authentication/authorization")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpPost("signin")]
    public async Task<IActionResult> SignIn(SignInUserDTO signInUserDTO)
    {
        var user = userManager.Users.FirstOrDefault(u => u.Email == signInUserDTO.Email);
        if (user == null) return Unauthorized();
        var res = await userManager.CheckPasswordAsync(user, signInUserDTO.Password);
        if (res)
        {
            var roles = (await userManager.GetRolesAsync(user)).ToArray();
            return Ok(new { token = await jwtProvider.GenerateJWT(user) });
        }
        return Unauthorized();
        
    }
}