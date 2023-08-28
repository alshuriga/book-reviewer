namespace BookReviewer.Shared.MongoDb;

public class MongoDbSettings 
{
    public string Host { get; init; }
    public string DatabaseName {get; init;}

    public string ConnectionString => $"mongodb://{Host}";
}
