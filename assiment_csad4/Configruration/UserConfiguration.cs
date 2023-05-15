using assiment_csad4.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace assiment_csad4.Configruration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(x => x.Id).HasDefaultValueSql("(newid())");
            builder.ToTable("User");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Status).HasColumnType("int").HasDefaultValueSql("((0))").IsRequired();
            builder.Property(x => x.Pass).HasColumnType("nvarchar(256)").IsRequired();
            builder.Property(x => x.Name).HasColumnType("nvarchar(256)").IsRequired();
            // khoa ngoai trong bang cart tro toi bang user
            builder.HasMany(x => x.Bills).WithOne(x => x.UserNavigation).HasForeignKey(p => p.UserId);
            builder.HasOne(x => x.RoleNavigation).WithMany(x => x.Users).HasForeignKey(p => p.IdRole);
            builder.HasIndex(p => p.Name); // Unique
            
        }
    }
}
