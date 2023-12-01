using Calabonga.OperationResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace Server.SignalR.Infrastructure.Attributes
{
    /// <summary>
    /// Пользовательский обработчик проверки доступности
    /// </summary>
    public class ValidatorModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.ModelState.IsValid)
            {
                return;
            }

            var operation = OperationResult.CreateResult(context.ModelState);
            var messages = context.ModelState.Values.SelectMany
                (x => x.Errors.Select(y => y.ErrorMessage));
            var message = string.Join(" ", messages);
            operation.AddError(message);
            context.Result = new OkObjectResult(operation);
        }
    }
}
