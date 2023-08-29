namespace BookReviewer.Shared.MongoDb;

public class MongoDbSettings 
{
    public required string Host { get; init; }
    public required string DatabaseName {get; init;}
    
    public string ConnectionString => $"mongodb://{Host}";
}
