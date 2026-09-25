using Refit;

namespace Gateway.Middlewares;

public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (ApiException ex)
        {
            logger.LogError(ex.Message, "  ", ex.Content);
            context.Response.StatusCode = (int)ex.StatusCode;
            await context.Response.WriteAsync(ex.Content ?? ex.Message);
        }
    }
}