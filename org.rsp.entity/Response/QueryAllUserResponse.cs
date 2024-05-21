using org.rsp.entity.Model;

namespace org.rsp.entity.Response;

public class QueryAllUserResponse
{
    public List<UserModel> UserModels { get; set; } = new();
    public int TotalCount { get; set; }
}