using AspNetCore.Identity.Mongo;
using BookReviewer.InentityProvider.IdentityModels;
using BookReviewer.Shared.MongoDb;

namespace BookReviewer.InentityProvider;

public static class Extensions
{
    public static IServiceCollection AddMongoIdentity(this IServiceCollection services, IConfiguration configuration)
    {
        var mongoSettings = configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
        services.AddIdentityMongoDbProvider<ApplicationUser, ApplicationRole, Guid>(identity =>
        {
            identity.Password.RequiredLength = 8;
            identity.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!#$%&'*+-/=?^_`{|}~.\"(),:;<>@[\\]";
            identity.User.RequireUniqueEmail = true;
        }, 
        mongo => {
            mongo.ConnectionString = $"{mongoSettings!.ConnectionString}/{mongoSettings.DatabaseName}";
        });
        return services;
    }
}