using Microsoft.EntityFrameworkCore;
using MyCompany.MyApp.UserAdministrator.Context;
using MyCompany.MyApp.UserAdministrator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCompany.MyApp.UserAdministrator.Repository
{
    public class UserRepo : IUserRepo
    {
        UserContext _context;

        public UserRepo(UserContext context)
        {
            _context = context;
        }

        public async Task CreateUser(User user)
        {
            if (user == null)
                throw new ArgumentException(nameof(user));

            await _context.Users.AddAsync(user);
        }

        public async Task<User> GetUserById(int id)
        {
            var res= await _context.Users.Include(u => u.UserRoles).ThenInclude(ur => ur.Role).FirstOrDefaultAsync(x => x.Id == id);
            return res;
                //new User { Id = 0, Login = "MyLogin", Name = "MyName" };
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<bool> SaveChanges()
        {
            return (await _context.SaveChangesAsync()>=0);
        }

        public void UpdateUser(User user)
        {
            _context.Users.Update(user);
        }

        public void DeleteUser(User user)
        {
            if (user == null)
                throw new ArgumentException(nameof(user));
            _context.Users.Remove(user);
        }
        
    }
}
