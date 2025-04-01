using LiteDB;

namespace X.Services;

public class LiteService
{
    private readonly LiteDatabase _db;
    private readonly ILiteCollection<BsonDocument> _usersCollection;

    public LiteService()
    {
        _db = new LiteDatabase("mydb.db");
        _usersCollection = _db.GetCollection<BsonDocument>("data");
    }
    public string? Get(string Token){
        var doc = _usersCollection.FindOne(x => x["Token"] == Token);
        if (doc != null && doc["expirationDate"].AsDateTime > DateTime.Now)
        {
            return doc["UserId"].AsString;
        }
        else if (doc != null && doc["expirationDate"].AsDateTime < DateTime.Now)
        {
            _usersCollection.Delete(doc["_id"]);
        }
        return null;
    }
    public bool Add(string Token, string UserId, DateTime expirationDate)
    {
        var doc = new BsonDocument
        {
            ["Token"] = Token,
            ["UserId"] = UserId,
            ["expirationDate"] = expirationDate
        };
        _usersCollection.Insert(doc);
        return true;
    }
    public bool Remove(string Token)
    {
        var doc = _usersCollection.FindOne(x => x["Token"] == Token);
        if (doc != null)
        {
            _usersCollection.Delete(doc["_id"]);
            return true;
        }
        return false;
    }
}