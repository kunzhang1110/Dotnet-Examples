using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;


namespace ApiExamples.Shared
{
    public class CustomActionFilterAttribute : ActionFilterAttribute
    {
        private readonly Stopwatch _stopwatch;
        private readonly string _message;

        public CustomActionFilterAttribute(string message)
        {
            _stopwatch = new Stopwatch();
            _message = message;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // run before the action executes
            _stopwatch.Start();
            context.HttpContext.Response.Headers.Add(
                "X-CustomActionFilter-OnActionExecuting",
                JsonSerializer.Serialize(context.ActionArguments));
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            // run after the action executes
            _stopwatch.Stop();
            var elapsedTime = _stopwatch.ElapsedMilliseconds;

            if (context.Result is ObjectResult objectResult)
            {
                objectResult.Value = new
                {
                    OriginalResponse = objectResult.Value,
                    ExecutionTime = elapsedTime,
                    message = _message
                };
            }
        }
    }
}
