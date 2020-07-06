using Microsoft.EntityFrameworkCore;
using MyCompany.MyApp.UserAdministrator.Context;
using MyCompany.MyApp.UserAdministrator.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyCompany.MyApp.UserAdministrator.Repository
{
    public class RoleRepo : IRoleRepo
    {
        UserContext _context;

        public RoleRepo(UserContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Role>> GetRoles()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task<Role> GetRoleById(int id)
        {
            var res = await _context.Roles.FirstOrDefaultAsync(x => x.Id == id);
            return res;
        }
    }
}
