using CRM.Core.Implement;

namespace CRM.WebApi
{
    public class ExceptionMiddleware : IMiddleware //Миддлвер реализует миддлвер, ауф!
    {
        public async Task
            InvokeAsync(HttpContext context,
                RequestDelegate next) // Метод, который пытается запустить проходящий запрос, и ловит ошибки
        {
            try
            {
                await next(context); // пробуем запустить
            }
            catch (Exception ex) // если случилось что то
            {
                await HandleExceptionAsync(context, ex); //тогда обрабатываем ошибку
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception) // метод обработки ошибки
        {
            //Статус код и сообщение ошибки объявим сразу
            
            const string errorMessage = "У нас что то пошло не так";
            const int errorCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = errorCode;
            //обьявляем наш ответ юзеру, тут это объект где пока только сообщение, но потом если нужно это возможно будет расширить
            var response = new InternalError(errorMessage);
            //отправляем браузеру ответ
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}