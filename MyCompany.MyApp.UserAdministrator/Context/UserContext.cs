using Microsoft.EntityFrameworkCore;
using MyCompany.MyApp.UserAdministrator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCompany.MyApp.UserAdministrator.Context
{
    public class UserContext : DbContext
    {
        public UserContext( DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasAlternateKey(u => u.Login);
            modelBuilder.Entity<User>().HasData(new User { Id = 1, Name = "Admin", Login = "Admin", Password = "Admin" });
            modelBuilder.Entity<Role>().HasAlternateKey(r => r.Name);
            modelBuilder.Entity<Role>().HasData(new Role {Id=1, Name = "Admin" });
            modelBuilder.Entity<Role>().HasData(new Role {Id=2, Name = "Guest" });
            modelBuilder.Entity<Role>().HasData(new Role {Id=3, Name = "Editor" });
            modelBuilder.Entity<UserRole>().HasData(new UserRole { Id = 1, RoleId = 1, UserId = 1 });
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        
        // many-to-many links table
        public DbSet<UserRole> UserRoles { get; set; }
    }
}
