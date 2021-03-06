﻿using MyCompany.MyApp.UserAdministrator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCompany.MyApp.UserAdministrator.Repository
{
    /// <summary>
    /// user/user properties operations
    /// </summary>
    public interface IUserRepo
    {
        Task<bool> SaveChanges();
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUserById(int id);
        Task CreateUser(User user);
        void UpdateUser(User user);
        void DeleteUser(User user);
        void SetUserRole(UserRole model);
        void RemoveUserRole(UserRole model);

    }
}
