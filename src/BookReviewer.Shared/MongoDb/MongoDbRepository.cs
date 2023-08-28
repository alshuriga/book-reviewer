using System.Linq.Expressions;
using BookReviewer.Shared.Entities;
using BookReviewer.Shared.Repositories;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BookReviewer.Shared.MongoDb;

public class MongoDbRepository<T> : IRepository<T> where T : Entity
{
    private readonly IMongoCollection<T> collection;
    private readonly FilterDefinitionBuilder<T> filterBuilder;
    public MongoDbRepository(IMongoDatabase mongoDatabase, IOptions<MongoDbSettings> options)
    {
        collection = mongoDatabase.GetCollection<T>(typeof(T).Name);
        filterBuilder = new FilterDefinitionBuilder<T>();
    }
    public async Task CreateAsync(T entity)
    {
        await collection.InsertOneAsync(entity);
    }

    public async Task DeleteAsync(Guid id)
    {
        await collection.DeleteOneAsync(filterBuilder.Eq(x => x.Id, id));
    }

    public async Task<IEnumerable<T>> GetAsync()
    {
        return await collection.Find(filterBuilder.Empty).ToListAsync();
    }

    public async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> expression)
    {
        return await collection.Find(expression).ToListAsync();
    }

    public async Task<T> GetByIdAsync(Guid id)
    {
        return await collection.Find(filterBuilder.Eq(x => x.Id, id)).FirstOrDefaultAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        if(entity == null)
            throw new ArgumentNullException("Entity is null");
        
        await collection.ReplaceOneAsync(filterBuilder.Eq(x => x.Id, entity.Id), entity);
    }
}