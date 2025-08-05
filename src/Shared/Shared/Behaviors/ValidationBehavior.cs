using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Shared.Contracts.CQRS;

namespace Shared.Behaviors;

public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse> 
	where TRequest : ICommand<TResponse>
{
	public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
	{
		ValidationContext<TRequest> context = new ValidationContext<TRequest>(request);
		
		ValidationResult[] validationResults = await Task.WhenAll(validators.Select(v => 
			v.ValidateAsync(context, cancellationToken))
		);
		
		ValidationFailure[] failures = validationResults
			.Where(r => r.Errors.Any())
			.SelectMany(r => r.Errors)
			.ToArray();

		if (failures.Any())
		{
			throw new ValidationException(failures);
		}
		
		return await next(cancellationToken);
	}
}