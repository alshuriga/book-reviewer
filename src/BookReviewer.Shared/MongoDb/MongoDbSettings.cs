namespace BookReviewer.Shared.MongoDb;

public class MongoDbSettings 
{
    public string Host { get; init; } = null!;
    public string DatabaseName {get; init;} = null!;

    public string ConnectionString => $"mongodb://{Host}";
}
