namespace CE.Assessment.WebApi.Middlewares
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _requestDelegate;
        private const string ApiKey = "XApiKey";

        public ApiKeyMiddleware(RequestDelegate requestDelegate)
        {
            _requestDelegate = requestDelegate;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if(!context.Request.Headers.TryGetValue(ApiKey, out var apiKeyValue))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Api Key is not provided");
                return;
            }

            var appSettings = context.RequestServices.GetRequiredService<IConfiguration>();
            var apiKey = appSettings.GetValue<string>(ApiKey);
            if (!apiKey.Equals(apiKeyValue))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized");
                return;
            }

            await _requestDelegate(context);
        }
    }
}
