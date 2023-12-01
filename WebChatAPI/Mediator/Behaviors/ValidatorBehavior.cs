using Calabonga.OperationResults;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WebChatAPI.Mediator.Behaviors
{
    public class ValidatorBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        public ValidatorBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var failures = _validators
                .Select(x => x.Validate(new ValidationContext<TRequest>(request)))
                .SelectMany(x => x.Errors)
                .Where(x => x != null)
                .ToList();
            if (!failures.Any())
            {
                return next();
            }

            var type = typeof(TResponse);
            if (!type.IsSubclassOf(typeof(OperationResult)))
            {
                var exception = new ValidationException(failures);
                throw exception;
            }

            var result = Activator.CreateInstance(type);
            (result as OperationResult).AddError(new Exception());
            foreach (var failure in failures)
            {
                (result as OperationResult)?.AppendLog($"{failure.PropertyName}: {failure.ErrorMessage}");
            }
            return Task.FromResult((TResponse)result);
        }
    }
}
