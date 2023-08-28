using AspNetCore.Identity.Mongo.Model;

namespace BookReviewer.InentityProvider.IdentityModels;

public class ApplicationUser : MongoUser<Guid>
{
}