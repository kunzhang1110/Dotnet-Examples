public class CustomMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<CustomMiddleware> _logger;

    public CustomMiddleware(RequestDelegate next, ILogger<CustomMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Execute when receiving a request 
        var request = context.Request;
        var response = context.Response;
        _logger.LogInformation($"Request Method: {request.Method}, Path: {request.Path}");

        // Add a custom response header, this needs to be done during receiving the request
        context.Response.Headers.Add("X-Custom-Header", "Hello from custom middleware!");

        // Call the next middleware in the pipeline
        await _next(context);

        // Execute when sending response 
        _logger.LogInformation($"Response Status Code: {response.StatusCode}");
    }
}