using AspNetCore.Identity.Mongo;
using BookReviewer.IdentityProvider.IdentityModels;
using BookReviewer.Shared.MongoDb;
using Microsoft.AspNetCore.Identity;

namespace BookReviewer.IdentityProvider;

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

        var uManager = app.Services.CreateScope().ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        var rManager = app.Services.CreateScope().ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

        await rManager.CreateAsync(new ApplicationRole() { Name = "Admin"} );

        var user = uManager.Users.FirstOrDefault(u => u.UserName == credentials!.Username);

        if(user == null)
        {
            var res = await uManager.CreateAsync(new ApplicationUser() { UserName = credentials!.Username, Email = credentials!.Username }, credentials.Password);
            if(!res.Succeeded) 
                throw new ApplicationException($"Error while creating an initial administrator account:\n{string.Join('\n', res.Errors)}");
            
            res = await uManager.AddToRoleAsync((await uManager.FindByNameAsync(credentials.Username))!, "Admin");
        }
    }
}