using System.Security.Claims;
using BookReviewer.Email.Service.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookReviewer.Email.Service.Controllers;

[ApiController]
[Route("email")]
public class EmailController : ControllerBase
{
    private readonly IEmailSubscribersRepository emailSubscribersRepository;
    private readonly IEmailSendingService emailSendingService;

    public EmailController(IEmailSubscribersRepository emailSubscribersRepository, IEmailSendingService emailSendingService)
    {
        this.emailSubscribersRepository = emailSubscribersRepository;
        this.emailSendingService = emailSendingService;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> SubscribeUserToBookReviews(SubscribeUserToBookDTO subscribeUserToBookDTO)
    {
        var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        
        if(email == null)
            return BadRequest("User doesn't have an email linked to the account");
            
        await emailSubscribersRepository.AddEmailToSubscribersAsync(email, subscribeUserToBookDTO.bookId);
        return NoContent();
    }

}