using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pos.Web.Data.Entities;

namespace Pos.Web.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Categories> Categories { get; set; }
        public DbSet<Sales> Sales { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<SalesDetail> SalesDetail { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<PrivatePosRole> PrivatePosRoles { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureIndexes(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private void ConfigureIndexes(ModelBuilder modelBuilder)
        {
            //SalesDetail
            modelBuilder.
                Entity<SalesDetail>().
                HasKey(x => new { x.SalesId, x.ProductId });

            // Roles
            modelBuilder.Entity<PrivatePosRole>()
                        .HasIndex(s => s.Name)
                        .IsUnique();
            // Users
            modelBuilder.Entity<User>()
                        .HasIndex(s => s.Document)
                        .IsUnique();

            // Role Permission
            modelBuilder.Entity<RolePermission>()
                        .HasKey(rs => new { rs.RoleId, rs.PermissionId });

            modelBuilder.Entity<RolePermission>()
                        .HasOne(rs => rs.Role)
                        .WithMany(r => r.RolePermissions)
                        .HasForeignKey(rs => rs.RoleId);

            modelBuilder.Entity<RolePermission>()
                        .HasOne(rs => rs.Permission)
                        .WithMany(s => s.RolePermissions)
                        .HasForeignKey(rs => rs.PermissionId);
        }

    }
}
