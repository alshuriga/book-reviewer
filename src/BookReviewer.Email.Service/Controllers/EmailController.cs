using System.Security.Claims;
using BookReviewer.Email.DTO;
using BookReviewer.Email.Service.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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