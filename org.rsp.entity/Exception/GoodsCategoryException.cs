namespace org.rsp.entity.Exception;

public class GoodsCategoryException : System.Exception
{
    public GoodsCategoryException(System.Exception innerEx, string message)
        : base(message,innerEx)
    { }

    public GoodsCategoryException(string message)
        : base(message)
    { }

    public const long UnknownErrCode = 00000001;
    
}