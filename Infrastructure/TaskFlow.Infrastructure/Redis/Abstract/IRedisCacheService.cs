namespace TaskFlow.Infrastructure.Redis.Abstract;

public interface IRedisCacheService
{
    Task SetTaskAsync(string id, string json);
    Task<string?> GetTaskAsync(string id);
}
