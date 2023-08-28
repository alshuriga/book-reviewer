using BookReviewer.Shared.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace BookReviewer.Shared.MongoDb;

public static class MongoDbExtensions 
{
    public static IServiceCollection AddMongoDbDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
        services.AddSingleton<IMongoDatabase>(opts => {
            var client = new MongoClient(settings.ConnectionString);
            var db = client.GetDatabase(settings.DatabaseName);
            return db;
        });

        return services;
    }
}