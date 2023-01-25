using System.Text.RegularExpressions;

namespace AirlineManager.API
{
    public class UserScopeMiddleware
    {
        private RequestDelegate _next;
        private ILogger<UserScopeMiddleware> _logger;

        public UserScopeMiddleware(RequestDelegate next, ILogger<UserScopeMiddleware> logger) 
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.User.Identity is {  IsAuthenticated: true })
            {
                var user = context.User;
                var pattern = @"(<=[\w]{1})[\w-\._\+%]*(?=[\w]{1}@)";
                var maskedUsername = Regex.Replace(user.Identity.Name ?? "", pattern, m => new string('*', m.Length));

                var subjectID = user.Claims.First(c => c.Type == "sub")?.Value;

                using (_logger.BeginScope("User:{user}", maskedUsername))
                {
                    await _next(context);
                }


            }
            else
            {
                await _next(context);
            }

        }
       /* [LoggerMessage(1, LogLevel.Information, "made it to the reservation room")]
        partial void Log();*/
    }
}
