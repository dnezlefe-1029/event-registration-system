using EventReg.Application.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EventReg.Api.Filters;

public class ApiResponseFilter : IAsyncResultFilter
{
    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        if (context.Result is ObjectResult objectResult && objectResult.Value is not null)
        {
            var statusCode = objectResult.StatusCode ?? StatusCodes.Status200OK;
            var value = objectResult.Value;
            var valueType = value.GetType();

            if (valueType.IsGenericType &&
                typeof(System.Collections.IEnumerable).IsAssignableFrom(valueType) &&
                valueType.GetGenericTypeDefinition() != typeof(Dictionary<,>))
            {
                var elementType = valueType.GetGenericArguments().First();

                var listType = typeof(List<>).MakeGenericType(elementType);
                var listValue = Activator.CreateInstance(listType, value);

                var genericType = typeof(ApiResponse<>).MakeGenericType(listType);
                var method = genericType.GetMethod(
                    nameof(ApiResponse<object>.Success),
                    new[] { listType, typeof(string), typeof(int) });

                var wrapped = method!.Invoke(null, new object[] { listValue!, "Success", statusCode });
                context.Result = new ObjectResult(wrapped) { StatusCode = statusCode };
            }
            else
            {
                var genericType = typeof(ApiResponse<>).MakeGenericType(valueType);
                var method = genericType.GetMethod(
                    nameof(ApiResponse<object>.Success),
                    new[] { valueType, typeof(string), typeof(int) });

                var wrapped = method!.Invoke(null, new object[] { value, "Success", statusCode });
                context.Result = new ObjectResult(wrapped) { StatusCode = statusCode };
            }
        }
        else if (context.Result is ObjectResult objResult) // Ok(null)
        {
            var statusCode = objResult.StatusCode ?? StatusCodes.Status200OK;
            var wrapped = ApiResponse<object>.Success(null, "Success", statusCode);
            context.Result = new ObjectResult(wrapped) { StatusCode = statusCode };
        }
        else if (context.Result is StatusCodeResult statusCodeResult)
        {
            var statusCode = statusCodeResult.StatusCode;
            var wrapped = statusCode >= 200 && statusCode < 300
                ? ApiResponse<object>.Success(null, "Success", statusCode)
                : ApiResponse<object>.Failed("Error", statusCode);

            context.Result = new ObjectResult(wrapped) { StatusCode = statusCode };
        }
        else if (context.Result is EmptyResult)
        {
            var wrapped = ApiResponse<object>.Success(null, "Success", StatusCodes.Status200OK);
            context.Result = new ObjectResult(wrapped) { StatusCode = StatusCodes.Status200OK };
        }
        else if (context.Result is null)
        {
            var wrapped = ApiResponse<object>.Success(null, "Success", StatusCodes.Status200OK);
            context.Result = new ObjectResult(wrapped) { StatusCode = StatusCodes.Status200OK };
        }

        await next();
    }
}

