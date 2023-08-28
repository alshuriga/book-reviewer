namespace BookReviewer.Email.Service.Repositories;

public interface IEmailSubscribersRepository
{
    Task<IEnumerable<string>> GetEmailsForBookAsync(Guid bookId);
    Task AddEmailToSubscribersAsync(string email, Guid bookId);
}