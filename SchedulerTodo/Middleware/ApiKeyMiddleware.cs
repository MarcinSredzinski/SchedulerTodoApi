using SchedulerTodo.Services;

namespace SchedulerTodo.Middleware;

public class ApiKeyMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, IApiKeyService apiKeyService)
    {
        if (!context.Request.Headers.TryGetValue("ApiKey", out var extractedApiKey))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("ApiKey is missing");
            return;
        }

        if (!apiKeyService.ValidateApiKey(extractedApiKey!))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("ApiKey is invalid");
            return;
        }

        await next(context);
    }
}