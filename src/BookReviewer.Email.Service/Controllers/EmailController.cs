using System.Security.Claims;
using BookReviewer.Email.DTO;
using BookReviewer.Email.Service.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BookReviewer.Email.Service.Controllers;

[ApiController]
[Route("email")]
public class EmailController : ControllerBase
{
    private readonly IEmailSubscribersRepository emailSubscribersRepository;

    public EmailController(IEmailSubscribersRepository emailSubscribersRepository)
    {
        this.emailSubscribersRepository = emailSubscribersRepository;
    }

    [SwaggerOperation("Subscribe a user to new reviews of specific book")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> SubscribeUserToBookReviews(SubscribeUserToBookDTO subscribeUserToBookDTO)
    {
        var email = User.FindFirstValue(ClaimTypes.Email);
        
        if(email == null)
            return BadRequest("User doesn't have an email linked to the account");
            
        await emailSubscribersRepository.AddEmailToSubscribersAsync(email, subscribeUserToBookDTO.BookId);
        return NoContent();
    }

}