using RocksDbSharp;

namespace X.Services;


public interface IRocksService
{
    void Add(string key, string value,DateTime expiry);
    string? Get(string key);
    void Delete(string key);
}

public class RocksService(RocksDb db) : IRocksService
{
    private readonly RocksDb _db = db;
    public void Add(string key, string value,DateTime expiry)
    {
        _db.Put(key, $"{value}::{expiry:o}");
    }
    public string? Get(string key)
    {
        string data = _db.Get(key);
        if(data == null) return null;
        List<string> parts = [.. data.Split("::")];
        if (parts.Count() == 2 && DateTime.TryParse(parts[1], out var expiry))
        {
            if (expiry > DateTime.UtcNow)
            {
                return parts[0]; // Return token if not expired
            }
            else
            {
                Delete(key); // Expired, remove from DB
            }
        }
        return null;
    }
    public void Delete(string key)
    {
        _db.Remove(key);
    }
}