using assiment_csad4.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace assiment_csad4.Configruration
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.Property(x => x.Id).HasDefaultValueSql("(newid())");
            builder.ToTable("Role");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Status).HasColumnType("int").HasDefaultValueSql("((0))").IsRequired();
            builder.Property(x => x.Description).HasColumnType("nvarchar(256)").IsRequired();
            builder.Property(x => x.Name).HasColumnType("nvarchar(256)").IsRequired();
            builder.HasIndex(p => p.Name); // Unique
        }
    }
}
