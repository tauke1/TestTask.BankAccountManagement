using System.Net;
using TestTask.BankAccountManagement.BAL.Exceptions;
using TestTask.BankAccountManagement.Models;

namespace TestTask.BankAccountManagement.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var message = "Something wrong happened";
            if (exception is UserFriendlyMessageException typedException)
            {
                message = typedException.Message;
                context.Response.StatusCode = (int)typedException.StatusCode;
            }

            await context.Response.WriteAsync(new ErrorDetailsDto()
            {
                StatusCode = context.Response.StatusCode,
                Message = message
            }.ToString());
        }
    }
}
