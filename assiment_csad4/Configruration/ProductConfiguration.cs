using assiment_csad4.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace assiment_csad4.Configruration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Product");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasDefaultValueSql("(newid())");
            builder.Property(x => x.Status).HasColumnType("int").HasDefaultValueSql("((0))");
            builder.Property(x => x.Name).HasColumnType("nvarchar(256)");
            builder.Property(x => x.Price).HasColumnType("money");
            builder.Property(x => x.Supplier).HasColumnType("nvarchar(256)");
            builder.Property(x => x.Description).HasColumnType("nvarchar(256)");
            builder.Property(x => x.AvailableQuantity).HasColumnType("int").HasDefaultValueSql("0");
            


        }
    }
}
