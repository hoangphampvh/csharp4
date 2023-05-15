using assiment_csad4.Models;
using Microsoft.EntityFrameworkCore;
using SellerProduct.Configurations;
using System.Reflection;

namespace assiment_csad4.Configruration
{
    public class MyDbContext : DbContext
    {
        public MyDbContext()
        {

        }
        public MyDbContext(DbContextOptions options) : base(options)
        {

        }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<CartDetail> CartDetails { get; set; }
        public virtual DbSet<Bill> Bills { get; set; }
        public virtual DbSet<BillDetail> BillDetails { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-3S50L70\\SQLEXPRESS;" +
            "Initial Catalog=AssimentAsp;Persist Security Info=True;User ID=hoangpham;Password=19112002");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        }
    }
}
//dotnet aspnet-codegenerator controller -name RoleController -namespace assiment_csad4.Controllers -m assiment_csad4.Models.Role -udl -dc assiment_csad4.Configruration.MyDbContext -outDir  