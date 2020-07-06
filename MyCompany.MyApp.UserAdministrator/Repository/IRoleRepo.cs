﻿using MyCompany.MyApp.UserAdministrator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCompany.MyApp.UserAdministrator.Repository
{
    public interface IRoleRepo
    {
        Task<IEnumerable<Role>> GetRoles();
        Task<Role> GetRoleById(int id);
    }
}
