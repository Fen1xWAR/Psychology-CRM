namespace CRM.WebApi
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
            int errorCode;
            string errorMessage;
            switch(exception)
            {
                case InvalidOperationException:
                    errorCode = StatusCodes.Status404NotFound;
                    errorMessage = "По вашему запросу ничего не найдено";
                    break;
                default: 
                    errorCode = StatusCodes.Status500InternalServerError;
                    errorMessage = "У нас что то пошло нет так";
                    break;
            }
             

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = errorCode;

            var response = new { message = errorMessage };
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}