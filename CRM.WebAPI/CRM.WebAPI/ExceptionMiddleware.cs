namespace TimePlanner.WebApi
{
    public class ExceptionMiddleware : IMiddleware
    {

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var errorCode = StatusCodes.Status500InternalServerError;
            var errorMessage = "У нас что то пошло не так";

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = errorCode;

            var response = new { message = errorMessage };
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}