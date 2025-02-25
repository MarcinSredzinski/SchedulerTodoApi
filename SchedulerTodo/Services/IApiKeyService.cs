namespace SchedulerTodo.Services;

public interface IApiKeyService
{
    bool ValidateApiKey(string apiKey);
}