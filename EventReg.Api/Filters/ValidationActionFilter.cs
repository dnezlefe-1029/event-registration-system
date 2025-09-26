using EventReg.Application.Common;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EventReg.Api.Filters;

public class ValidationActionFilter : IAsyncActionFilter
{
    private readonly IServiceProvider _serviceProvider;

    public ValidationActionFilter(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        foreach (var arg in context.ActionArguments.Values)
        {
            if (arg is null) continue;

            var argType = arg.GetType();
            var validatorType = typeof(IValidator<>).MakeGenericType(argType);
            var validator = _serviceProvider.GetService(validatorType) as IValidator;

            if (validator is null) continue;

            ValidationResult result = await validator.ValidateAsync(new ValidationContext<object>(arg));

            if (!result.IsValid)
            {
                var errors = result.Errors.Select(e => new ApiError
                {
                    Field = e.PropertyName,
                    Error = e.ErrorMessage
                });

                var response = ApiResponse<object>.Failed("Validation failed", 400, errors);
                context.Result = new JsonResult(response) { StatusCode = 400 };
                return;
            }
        }

        await next();
    }
}
