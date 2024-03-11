using org.rsp.entity.Common;

namespace org.rsp.entity.Request;

public class QueryCategoryByConditionRequest
{
    public string Name { get; set; }
    
    public string Desctiption { get; set; }

    public Pager Pager { get; set; } = new ();
}