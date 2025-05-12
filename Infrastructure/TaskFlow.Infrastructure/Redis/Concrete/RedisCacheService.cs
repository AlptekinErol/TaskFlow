using StackExchange.Redis;
using TaskFlow.Infrastructure.Redis.Abstract;

namespace TaskFlow.Infrastructure.Redis.Concrete;

public class RedisCacheService : IRedisCacheService
{
    private readonly IDatabase database;

    public RedisCacheService()
    {
        var redis = ConnectionMultiplexer.Connect("localhost");
        this.database = redis.GetDatabase();
    }

    public Task SetTaskAsync(string id, string json)
    {
        var expiry = TimeSpan.FromMinutes(10);
        return this.database.StringSetAsync($"task:{id}", json, expiry);
    }

    public async Task<string?> GetTaskAsync(string id)
    {
        var value = await this.database.StringGetAsync($"task:{id}");
        return value.HasValue ? value.ToString() : null;
    }
}
