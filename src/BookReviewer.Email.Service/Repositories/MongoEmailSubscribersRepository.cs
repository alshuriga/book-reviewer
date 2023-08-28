using BookReviewer.Email.Service.Entites;
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
    public async Task AddEmailToSubscribersAsync(string email, Guid reviewId)
    {
        var filter = filterBuilder.Eq(x => x.ReviewId, reviewId);
        var data = await collection.Find(filter).FirstOrDefaultAsync();

        if(data == null) {
            await collection.InsertOneAsync(new ReviewSubscriptionData() { ReviewId = reviewId, Emails = new List<string>() { email } });
            return;
        }       
        
        if(!data.Emails.Contains(email)) 
        {
            data.Emails.Add(email);
            await collection.FindOneAndReplaceAsync(filter, data);
        }
        return;
    }

    public async Task<IEnumerable<string>> GetEmailsForBookAsync(Guid reviewId)
    {
        var emails = (await collection.Find(s => s.ReviewId == reviewId).FirstOrDefaultAsync()).Emails ?? new List<string>();
        return emails;
    }
}