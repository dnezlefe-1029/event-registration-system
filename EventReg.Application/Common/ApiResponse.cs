namespace EventReg.Application.Common;

public class ApiResponse<T>
{
    public int StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public IEnumerable<ApiError>? Errors { get; set; }
    public static ApiResponse<T> Success(T data, string message = "Success", int statusCode = 200)
        => new() { StatusCode = statusCode, Message = message, Data = data };
    public static ApiResponse<T> Failed(string message, int statusCode, IEnumerable<ApiError>? errors = null)
        => new() { StatusCode = statusCode, Message = message, Errors = errors };
}

public class ApiError
{
    public string Field { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
