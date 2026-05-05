namespace MinhaApi.Models.Dtos;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public List<string>? Errors { get; set; }
    public List<string>? Logs { get; set; }

    public static ApiResponse<T> Ok(T data, List<string>? logs = null)
    {
        return new ApiResponse<T>
        {
            Success = true,
            Data = data,
            Logs = logs
        };
    }

    public static ApiResponse<T> Fail(List<string> errors)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Errors = errors
        };
    }
}
