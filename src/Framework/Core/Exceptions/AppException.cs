namespace Framework.Core.Exceptions;

public class AppException : Exception
{
    public int StatusCode { get; }
    public override string Message { get; }
    
    public AppException(string message) : base(message)
    {
        StatusCode = 400;
        Message = message!;
    }

    public AppException(int code = 400, string message = null)
    {
        StatusCode = code;
        Message = message!;
    }
}