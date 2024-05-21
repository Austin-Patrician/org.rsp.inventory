using org.rsp.entity.Common;

namespace org.rsp.entity.Request;

public class AddGoodsCategoryRequest : BaseFiled
{
    public string GoodsCategoryName { get; set; }
    
    public string? Description { get; set; }
}