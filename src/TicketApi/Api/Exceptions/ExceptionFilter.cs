using CoreLib.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Exceptions
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            var details = new ProblemDetails();

            if (exception is DomainException)
            {
                details.Title = "Проблема при вставке данных!";
                details.Status = StatusCodes.Status409Conflict;
            }
            if (exception is ServiceException)
            {
                details.Title = "Проблема в бизнес-логике!";
                details.Status = StatusCodes.Status409Conflict;
            }
            if (exception is EntityNotFoundException)
            {
                details.Title = "Ресурс не найден!";
                details.Status = StatusCodes.Status409Conflict;
            }
            if (exception is EntityExistsException)
            {
                details.Title = "Сущность с таким свойством найдена!";
                details.Status = StatusCodes.Status409Conflict;
            }
            if (exception is ArgumentException)
            {
                details.Title = "Некорреткно составлен запрос!";
                details.Status = StatusCodes.Status400BadRequest;
            }

            details.Detail ??= exception.Message;
            details.Instance = context.HttpContext.Request.Path;

            context.Result = new ObjectResult(details)
            {
                StatusCode = details.Status
            };
            context.ExceptionHandled = true;
        }
    }
}
