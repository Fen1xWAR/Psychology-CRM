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
            const int errorCode = StatusCodes.Status500InternalServerError;
            const string errorMessage = "У нас что то пошло не так";
            // switch (exception) //смотрим что за ошибка
            // {
            //     case InvalidOperationException
            //         : //если ошибка такая, что запрос ничего не нашел, выдаем юзеру соответсвующее уведомление
            //         errorCode = StatusCodes.Status404NotFound;
            //         errorMessage = "По вашему запросу ничего не найдено";
            //         break;
            //     default: //иначе просто говорим что мы напортачили 
            //         errorCode = StatusCodes.Status500InternalServerError;
            //         errorMessage = "У нас что то пошло нет так";
            //         break;
            // }

            //следующие две строки, просто устанавливают тип ответа браузера и код, который он получает (от нас :) )
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = errorCode;
            //обьявляем наш ответ юзеру, тут это объект где пока только сообщение, но потом если нужно это возможно будет расширить
            var response = new { message = errorMessage };
            //отправляем браузеру ответ
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}