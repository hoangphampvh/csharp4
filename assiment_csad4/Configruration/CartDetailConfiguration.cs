using assiment_csad4.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace assiment_csad4.Configruration
{
    public class CartDetailConfiguration : IEntityTypeConfiguration<CartDetail>
    {
        public void Configure(EntityTypeBuilder<CartDetail> builder)
        {
            builder.Property(x => x.Id).HasDefaultValueSql("(newid())");
            builder.ToTable("CartDetail");
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.CartNavigation).WithMany(x => x.CartDetails).HasForeignKey(x => x.IdUser);
            builder.HasOne(x => x.ProductNavigation).WithMany(x => x.CartDetails).HasForeignKey(x => x.IdProduct);
            builder.Property(p => p.Quantilty).HasColumnType("int").IsRequired().HasDefaultValueSql("1");

        }
    }
}
