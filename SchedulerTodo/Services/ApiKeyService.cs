using SchedulerTodo.DB;

namespace SchedulerTodo.Services;

public class ApiKeyService(SqlServerDbContext dbContext) : IApiKeyService
{
    public bool ValidateApiKey(string apiKey)
    {
        var key = dbContext.ApiKeys.SingleOrDefault(k => k.Key == apiKey);
        if (key is null)
            return false;
        if (key.Expiration is null)
            return true;
        return key.Expiration < DateTime.Now && false;
    }
}