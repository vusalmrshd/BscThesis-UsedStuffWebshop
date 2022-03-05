using System;
using Microsoft.EntityFrameworkCore;
using Models.Database.Entity;
using Models.Database.Sps;

namespace Models.Database.Context
{
    public class DatabaseContext : DbContext
    {

        public DbSet<User> users { get; set; }
        public DbSet<Role> roles { get; set; }
        public DbSet<UserRole> userRoles { get; set; }
        public DbSet<Permission> permissions { get; set; }
        public DbSet<UserPermission> userPermissions { get; set; }
        public DbSet<UserForgetPassword> userForgetPasswords { get; set; }

        public DbSet<Product> products { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<Order> orders { get; set; }
        public DbSet<OrderProducts> orderProducts { get; set; }


        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }


        //Before Running Migration Remove all Code in Model Creating
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region SPS

            modelBuilder.Entity<SpLoginResponse>().HasNoKey();
            modelBuilder.Entity<SpPermissionResponse>().HasNoKey();

            #endregion
            modelBuilder.Entity<User>()
            .Property(f => f.userId)
            .ValueGeneratedOnAdd();

            modelBuilder.Entity<Role>()
            .Property(f => f.roleId)
            .ValueGeneratedOnAdd();

            modelBuilder.Entity<Permission>()
            .Property(f => f.permissionId)
            .ValueGeneratedOnAdd();

            modelBuilder.Entity<UserRole>()
            .Property(f => f.id)
            .ValueGeneratedOnAdd();

            modelBuilder.Entity<UserPermission>()
            .Property(f => f.id)
            .ValueGeneratedOnAdd();

            modelBuilder.Entity<UserForgetPassword>()
            .Property(f => f.id)
            .ValueGeneratedOnAdd();

            modelBuilder.Entity<Product>()
           .Property(f => f.productId)
           .ValueGeneratedOnAdd();

            modelBuilder.Entity<Order>()
           .Property(f => f.orderId)
           .ValueGeneratedOnAdd();

            modelBuilder.Entity<OrderProducts>()
           .Property(f => f.id)
           .ValueGeneratedOnAdd();

            modelBuilder.Entity<Category>()
           .Property(f => f.id)
           .ValueGeneratedOnAdd();

        }
    }
}
