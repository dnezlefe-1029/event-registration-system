using Swashbuckle.AspNetCore.SwaggerGen;
using EventReg.Application.Common;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;

namespace EventReg.Api.Swagger;

public class ApiResponseOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var returnType = context.MethodInfo.ReturnType;

        if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>))
        {
            returnType = returnType.GetGenericArguments()[0];
        }

        if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(ActionResult<>))
        {
            var innerType = returnType.GetGenericArguments()[0];
            var apiResponseType = typeof(ApiResponse<>).MakeGenericType(innerType);

            AddResponse(operation, context, "200", "Success", apiResponseType, GetSuccessExample());
        }
        else if (returnType == typeof(IActionResult) || returnType == typeof(Task<IActionResult>))
        {
            AddResponse(operation, context, "200", "Success", typeof(ApiResponse<object>), GetSuccessExample());
        }
        else
        {
            AddResponse(operation, context, "200", "Success", typeof(ApiResponse<object>), GetSuccessExample());
        }

        AddResponse(operation, context, "400", "Bad Request", typeof(ApiResponse<object>), GetErrorExample(400, "Bad request"));
        AddResponse(operation, context, "401", "Unauthorized", typeof(ApiResponse<object>), GetErrorExample(401, "Unauthorized"));
        AddResponse(operation, context, "403", "Forbidden", typeof(ApiResponse<object>), GetErrorExample(403, "Forbidden"));
        AddResponse(operation, context, "404", "Not Found", typeof(ApiResponse<object>), GetErrorExample(404, "Not found"));
        AddResponse(operation, context, "500", "Server Error", typeof(ApiResponse<object>), GetErrorExample(500, "Server error"));
    }

    private void AddResponse(OpenApiOperation operation, OperationFilterContext context, string statusCode, string description, Type responseType, OpenApiObject example)
    {
        operation.Responses[statusCode] = new OpenApiResponse
        {
            Description = description,
            Content =
            {
                ["application/json"] = new OpenApiMediaType
                {
                    Schema = context.SchemaGenerator.GenerateSchema(responseType, context.SchemaRepository),
                    Example = example
                }
            }
        };
    }

    private OpenApiObject GetSuccessExample() => new OpenApiObject
    {
        ["statusCode"] = new OpenApiInteger(200),
        ["message"] = new OpenApiString("Success"),
        ["data"] = new OpenApiObject
        {
            ["id"] = new OpenApiInteger(1),
            ["name"] = new OpenApiString("Sample")
        },
        ["errors"] = new OpenApiNull()
    };

    private OpenApiObject GetErrorExample(int statusCode, string message) => new OpenApiObject
    {
        ["statusCode"] = new OpenApiInteger(statusCode),
        ["message"] = new OpenApiString(message),
        ["data"] = new OpenApiNull(),
        ["errors"] = new OpenApiArray
        {
            new OpenApiObject
            {
                ["field"] = new OpenApiString("Server"),
                ["error"] = new OpenApiString("An error occurred")
            }
        }
    };
}
