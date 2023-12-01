using Calabonga.AspNetCore.Controllers.Base;
using Calabonga.OperationResults;
using FluentValidation;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Server.SignalR.ConfigureServices.Extensions
{
    public class ValidatorBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validator;

        public ValidatorBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validator = validators;
        }

        /// <summary>
        /// Обработчик конвейера. Выполните любое дополнительное поведение
        /// и ожидайте делегата <paramref name="next" /> по мере необходимости.
        /// </summary>
        /// <param name="request">Входящий запрос</param>
        /// <param name="next">Ожидаемый делегат для следующего действия в конвейере.
        /// В конце концов этот делегат представляет обработчик.</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Ожидаемая задача, возвращающая <typeparamref name="TResponse" /></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var failures = _validator
                .Select(x => x.Validate(new ValidationContext<TRequest>(request)))
                .SelectMany(x => x.Errors)
                .Where(x => x != null)
                .ToList();
            if(!failures.Any())
            {
                return next();
            }
            if(request is RequestBase<OperationResult<TResponse>>)
            {
                var operations = OperationResult.CreateResult<TResponse>();
                operations.AddError(new ValidationException(failures));
            }
            return Task.FromResult(default(TResponse));
        }
    }
}
