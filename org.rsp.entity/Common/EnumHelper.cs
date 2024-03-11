using org.rsp.entity.Enum;

namespace org.rsp.entity.Common;

public class EnumHelper
{
    public static string? GetDescription(ResultStatus status)
    {
        switch (status)
        {
            case ResultStatus.Error:
                return "ERROR";
            case ResultStatus.Fail:
                return "FAIL";
            case ResultStatus.Success:
                return "SUCCESS";
        }

        return "";
    }
}