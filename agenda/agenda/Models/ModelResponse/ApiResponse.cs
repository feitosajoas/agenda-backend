namespace agenda.Models.ModelResponse;

public class ApiResponse
{
    public int StatusCode { get; set; }
    public string Message { get; set; }

    public ApiResponse(int statusCode, string message, object data = null)
    {
        StatusCode = statusCode;
        Message = message;
    }
}
