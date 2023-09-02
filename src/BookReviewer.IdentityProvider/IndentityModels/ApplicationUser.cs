using AspNetCore.Identity.Mongo.Model;

namespace BookReviewer.IdentityProvider.IdentityModels;

public class ApplicationUser : MongoUser<Guid>
{
}