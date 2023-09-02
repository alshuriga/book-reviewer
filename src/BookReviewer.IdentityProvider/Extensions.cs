using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Security.Claims;
using AspNetCore.Identity.Mongo;
using BookReviewer.InentityProvider.IdentityModels;
using BookReviewer.Shared.MongoDb;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Writers;

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

    public static async Task AddInitialAdminCredentials(this WebApplication app)
    {
        var credentials = app.Configuration.GetSection(nameof(InitialAdminUserCredentials)).Get<InitialAdminUserCredentials>();

        var usersService = app.Services.CreateScope().ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        var user = usersService.Users.FirstOrDefault(u => u.UserName == credentials!.Username);

        if(user == null)
            await usersService.CreateAsync(new ApplicationUser() { UserName = credentials!.Username}, credentials.Password);
    }
}