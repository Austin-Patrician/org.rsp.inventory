namespace org.rsp.entity.Exception;

public class RoleException: System.Exception
{
    public int ErrorCode { get; set; }
    
    public RoleException()
    {
    }

    public RoleException(string? message) : base(message)
    {
    }

    public RoleException(int errorCode)
    {
        ErrorCode = errorCode;
    }

    public RoleException(string? message, System.Exception? innerException) : base(message, innerException)
    {
    }
}