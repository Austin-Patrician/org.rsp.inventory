using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using org.rsp.database.Table;

namespace org.rsp.database.Mapping;

public class GoodsCategoryMapping: IEntityTypeConfiguration<GoodsCategory>
{
    public void Configure(EntityTypeBuilder<GoodsCategory> builder)
    {
        //配置实体类的属性
    }
}