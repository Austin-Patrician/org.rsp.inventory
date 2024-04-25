using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using org.rsp.database.Table;

namespace org.rsp.database.Mapping;

public class RoleMapping: IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.Property(_ => _.Name).IsRequired();
        builder.HasIndex(_ => _.Name).IsUnique();
    }
}