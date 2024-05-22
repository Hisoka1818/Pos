using Microsoft.EntityFrameworkCore;
using Pos.Web.Data.Entities;

namespace Pos.Web.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SalesDetail>().HasKey(x => new { x.SalesId, x.ProductId });

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Sales> Sales { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<SalesDetail> SalesDetail { get; set; }
        public DbSet<Customer> Customer { get; set; }

        public DbSet<Categories> Categories { get; set; }


    }
}
