using FluentValidation;
using MediatR;
using Project3.Domain.Common.Response;

namespace Project3.Application.Common.Behaviors;

// calcalke ro ar gavuketo injection handlerebshi validatorebs, globalurad davwer

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        // tu requestistvis validatori ar arsebobs, mashin daskipos eg
        if (!_validators.Any())
        {
            return await next();
        }

        
        var context = new ValidationContext<TRequest>(request);

        // validatorebi rom paralelurad gavushva
        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        // shedegebi ertad rom shegrovdes
        var failures = validationResults
            .Where(r => !r.IsValid)
            .SelectMany(r => r.Errors)
            .ToList();
        
        if (failures.Any())
        {
            return CreateValidationResult<TResponse>(failures);
        }

        // validacias rom gaivlis handlerze gadavides
        return await next();
    }

    private static TResult CreateValidationResult<TResult>(List<FluentValidation.Results.ValidationFailure> failures)
    {
        // yvela erroris shegroveba
        var errorMessages = string.Join("; ", failures.Select(f => f.ErrorMessage));

        
        if (typeof(TResult).IsGenericType && 
            typeof(TResult).GetGenericTypeDefinition() == typeof(Result<>))
        {
            var resultType = typeof(TResult).GetGenericArguments()[0];
            
            var failureMethod = typeof(Result<>)
                .MakeGenericType(resultType)
                .GetMethod(nameof(Result.Failure), new[] { typeof(string) });
            
            return (TResult)failureMethod!.Invoke(null, new object[] { errorMessages })!;
        }

        return (TResult)(object)Result.Failure(errorMessages);
    }
}