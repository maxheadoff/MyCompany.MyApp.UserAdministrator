﻿using AutoMapper;
using MyCompany.MyApp.UserAdministrator.Dtos;
using MyCompany.MyApp.UserAdministrator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCompany.MyApp.UserAdministrator.Profiles
{
    public class UsersProfile:Profile
    {
        public UsersProfile()
        {
            //getting data from DB
            CreateMap<User, UserReadDto>()
                .ForMember(u=>u.Roles,r=>r.MapFrom(s=>s.UserRoles.Select(item=>item.Role)));
            // creating new user
            CreateMap<UserCreateDto, User>();
            CreateMap<UserUpdateDto, User>();
            CreateMap<UserRole, UserRoleReadDto>();
            CreateMap<Role, RoleReadDto>();
        }
    }
}
