namespace org.rsp.entity.Exception;

public class UserException : System.Exception
{
    public int ErrorCode { get; set; }
    
    public UserException()
    {
    }

    public UserException(string? message) : base(message)
    {
    }

    public UserException(string? message, System.Exception? innerException) : base(message, innerException)
    {
    }

    public UserException(int errorCode)
    {
        ErrorCode = errorCode;
    }
}