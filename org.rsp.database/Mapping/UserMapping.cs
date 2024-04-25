using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using org.rsp.database.Table;

namespace org.rsp.database.Mapping;

public class UserMapping: IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(_ => _.Phone).IsRequired().HasMaxLength(11);
        builder.Property(_ => _.UserName).IsRequired().HasMaxLength(100);
        builder.Property(_ => _.PassWord).IsRequired().HasMaxLength(300);
        builder.Property(_ => _.Remark).HasMaxLength(200);
        builder.HasIndex(_ => _.Phone).IsUnique();
    }
}