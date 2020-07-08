using AutoMapper;
using Microsoft.Extensions.Logging;
using MyCompany.MyApp.UserAdministrator.Controllers;
using MyCompany.MyApp.UserAdministrator.Models;
using MyCompany.MyApp.UserAdministrator.Profiles;
using MyCompany.MyApp.UserAdministrator.Repository;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyCompany.MyApp.UserAdministrator.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void UserRolesTest()
        {
            var _repo = new MoqRepo();
            var _logger = new MoqLogger();
            var _mapper = new AutoMapper.Mapper(new MapperConfiguration(cfg=> {
                cfg.AddProfile(new UsersProfile());
            }));
            var controller = new UsersController(_repo, _logger, _mapper);
            var res=controller.PutRoles(1, new[] { 1, 2, 3,4 }).Result;
            Assert.NotNull(res);
            Assert.IsTrue(_repo.isSaved);
            Assert.IsTrue(_logger.IsValid);
            Assert.IsTrue(_repo.ToAdd.Count > 0);
            Assert.IsTrue(_repo.ToRemove.Count > 0);
        }
    }

    internal class MoqLogger : ILogger<UsersController>
    {
        public bool IsValid { get; private set; } = false;

        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            throw new NotImplementedException();
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            IsValid = true;
        }
    }

    internal class MoqRepo : IUserRepo
    {
        public List<int> ToAdd { get; private set; } = new List<int>();
        public List<int> ToRemove { get; private set; } = new List<int>();
        public bool isSaved { get; private set; }

        public Task CreateUser(User user)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteUser(User user)
        {
            throw new System.NotImplementedException();
        }

        public Task<User> GetUserById(int id)
        {
            return Task.FromResult(new User
            {
                Id = 1,
                Name = "TestUser",
                Roles = new UserRole[]
                {
                    new UserRole{RoleId=2 },
                    new UserRole{RoleId=5 },
                }
            });
        }

        public Task<IEnumerable<User>> GetUsers()
        {
            throw new System.NotImplementedException();
        }

        public void RemoveUserRole(UserRole model)
        {
            ToRemove.Add(model.RoleId);
        }

        public Task<bool> SaveChanges()
        {
            isSaved = true;
            return Task.FromResult(true);
        }

        public void SetUserRole(UserRole model)
        {
            ToAdd.Add(model.RoleId);
        }

        public void UpdateUser(User user)
        {
            throw new System.NotImplementedException();
        }
    }

}