using System.Text;

namespace org.rsp.entity.Common;

public static class SqlBindHelper
{
    public static string BindInSql(List<int> ids)
    {
        var builder = new StringBuilder();
        for (int i = 0; i < ids.Count; i++)
        {
            if (i != ids.Count -1)
            {
                builder.Append(ids[i] + ",");
            }
            else
            {
                builder.Append(ids[i]);
            }
        }

        return builder.ToString();
    }
}