namespace Framework.Core.Exceptions;

public class ApiResponse
{
    public int StatusCode { get; set; }
    public string Message { get; set; }

    public ApiResponse(int statusCode, string message = null!)
    {
        StatusCode = statusCode;
        Message = (message ?? GetDefaultMessageForStatusCode(statusCode))!;
    }

    private static string GetDefaultMessageForStatusCode(int statusCode)
    {
        return statusCode switch
        {
            200 => "Saved",
            201 => "Created",
            400 => "A bad request, you have made",
            401 => "Unauthorized request",
            404 => "Data not found",
            500 => "Errors are the path to the dark side. Errors lead to anger.  Anger leads to hate.  Hate leads to career change",
            _ => null
        };

    }
}