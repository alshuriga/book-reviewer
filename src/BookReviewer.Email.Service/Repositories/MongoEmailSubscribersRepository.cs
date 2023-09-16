using System.Globalization;
using BookReviewer.Email.DTO;
using BookReviewer.Email.Service.Entities;
using MassTransit.Initializers;
using MongoDB.Driver;

namespace BookReviewer.Email.Service.Repositories;

public class MongoEmailSubscribersRepository : IEmailSubscribersRepository
{
    private readonly IMongoCollection<ReviewSubscriptionData> collection;

    private readonly FilterDefinitionBuilder<ReviewSubscriptionData> filterBuilder;

    public MongoEmailSubscribersRepository(IMongoDatabase mongoDatabase)
    {
        collection = mongoDatabase.GetCollection<ReviewSubscriptionData>(nameof(ReviewSubscriptionData));
        filterBuilder = new FilterDefinitionBuilder<ReviewSubscriptionData>();
    }
    public async Task AddEmailToSubscribersAsync(string email, Guid bookId)
    {
        var filter = filterBuilder.Eq(x => x.BookId, bookId);
        var data = await collection.Find(filter).FirstOrDefaultAsync();

        if(data == null) {
            await collection.InsertOneAsync(new ReviewSubscriptionData() { BookId = bookId, Emails = new List<string>() { email } });
            return;
        }       
        
        if(!data.Emails.Contains(email)) 
        {
            data.Emails.Add(email);
            await collection.FindOneAndReplaceAsync(filter, data);
        }
        return;
    }

    public async Task RemoveEmailFromSubscribersAsync(string email, Guid bookId)
    {
        var filter = filterBuilder.Eq(x => x.BookId, bookId);
        var data = await collection.Find(filter).FirstOrDefaultAsync();       
        if(data != null) 
        {
            data.Emails.Remove(email);
            await collection.FindOneAndReplaceAsync(filter, data);
        }
        return;
    }


    public async Task<UserSubscriptionsDTO> GetBooksByEmailAsync(string email)
    {
       var filter = filterBuilder.AnyStringIn(x => x.Emails, email);

       var subscribedBooks = await collection.Find(filter).Project(p => p.BookId).ToListAsync();

       return new UserSubscriptionsDTO(subscribedBooks);
    }

    public async Task<IEnumerable<string>> GetEmailsForBookAsync(Guid reviewId)
    {
        var emails = (await collection.Find(s => s.BookId == reviewId).FirstOrDefaultAsync()).Emails ?? new List<string>();
        return emails;
    }

}
