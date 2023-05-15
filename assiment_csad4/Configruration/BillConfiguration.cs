using assiment_csad4.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace SellerProduct.Configurations
{
    public class BillConfiguration : IEntityTypeConfiguration<Bill>
    {
        public void Configure(EntityTypeBuilder<Bill> builder)
        {
            builder.ToTable("Bill"); // Đặt tên bảng
            builder.HasKey(p => new { p.Id });// Thiết lập khóa chính
            // Thiết lập cho các thuộc tính
            builder.Property(p => p.Status).HasColumnType("int").
                IsRequired(); // int not null
            builder.Property(x => x.Id).HasDefaultValueSql("(newid())");

        }
    }
}
