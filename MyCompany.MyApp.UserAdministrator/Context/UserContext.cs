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

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        
        // many-to-many links table
        public DbSet<UserRole> UserRoles { get; set; }
    }
}
