using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using MyCompany.MyApp.UserAdministrator.Dtos;
using MyCompany.MyApp.UserAdministrator.Models;
using MyCompany.MyApp.UserAdministrator.Repository;

namespace MyCompany.MyApp.UserAdministrator.Controllers
{
    
    /// <summary>
    /// operate user and it's  roles
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepo _repo = null;
        private readonly ILogger<UsersController> _logger;
        private readonly IMapper _mapper;

        public UsersController(IUserRepo repo, ILogger<UsersController> logger, IMapper mapper) : base()
        {
            _repo = repo;
            _logger = logger;
            _mapper = mapper;
        }

        // GET: api/<controller>
        [HttpGet]
        [Authorize()]
        public async Task<ActionResult<IEnumerable<UserReadDto>>> Get()
        {
            _logger.LogTrace("attempt to get users");
            return Ok(_mapper.Map<IEnumerable<UserReadDto>>(await _repo.GetUsers()));
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        [Authorize()]
        public async Task<ActionResult<UserReadDto>> GetById(int id)
        {
            var user = await _repo.GetUserById(id);
            if (user == null)
                return NotFound();
            _logger.LogTrace("attempt to get user,id=id");
            return Ok(_mapper.Map<UserReadDto>(user));
        }

        // POST api/<controller>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserReadDto>> Post([FromBody]UserCreateDto dto)
        {
            User model = null;
            try
            {
                model = _mapper.Map<User>(dto);
                await _repo.CreateUser(model);
                await _repo.SaveChanges();
                return CreatedAtAction(nameof(GetById), new { id = model.Id }, _mapper.Map<UserReadDto>(model));
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"user, name:{dto?.Name} create operation cause exception");
                throw ex;
            }
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Put(int id, UserUpdateDto dto)
        {
            User model=null;
            try
            {
                model = await _repo.GetUserById(id);
                if (model == null)
                    return NotFound();
                _mapper.Map(dto, model);
                _repo.UpdateUser(model);
                await _repo.SaveChanges();
                _logger.LogInformation($"User,name:{model.Name} updated");
                return NoContent();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"user:{model?.Name} update operation cause exception");
                throw ex;
            }
        }

        // PUT api/<controller>/5
        /// <summary>
        /// update list of roles for user
        /// </summary>
        /// <param name="id">user id</param>
        /// <param name="dto">array of role id to match to user</param>
        /// <returns></returns>
        [HttpPut("{id}/Roles")]
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult> PutRoles(int id, ICollection<int> dto)
        {
            try
            {
                var model = await _repo.GetUserById(id);
                if (model == null)
                    return NotFound();
                var removeList = from exists in model.Roles
                                 join requested in dto on exists.RoleId equals requested into med
                                 from aaa in med.DefaultIfEmpty()
                                 select new { Exists = exists, RequestedId = aaa };

                //Makes collection of delegates removes redundant role links
                var toRemove = removeList.Where(i => i.RequestedId == 0).Select(i => { return new Action(() => _repo.RemoveUserRole(i.Exists)); });

                var addList = from requested in dto
                              join exists in model.Roles on requested equals exists.RoleId into med
                              from aaa in med.DefaultIfEmpty()
                              select new { Exists = aaa, RequestedId = requested };

                //Makes collection of delegates adds new role links
                var toAdd = addList.Where(i => i.Exists == null)
                    .Select(i =>
                    {
                        return new Action(() => _repo.SetUserRole(new UserRole { RoleId = i.RequestedId, UserId = id }));
                    });
                //Invokes delegates
                foreach (var act in toRemove)
                    act.Invoke();
                foreach (var act in toAdd)
                    act.Invoke();
                await _repo.SaveChanges();
                _logger.LogInformation("UserRoles saved");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"error in {nameof(PutRoles)}");
                return StatusCode(500);
            }
        }


        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int id) 
        {
            User model = null;
            try
            {
                model = await _repo.GetUserById(id);
                if (model == null)
                    return NotFound();
                _repo.DeleteUser(model);
                await _repo.SaveChanges();
                return NoContent();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"user:{model?.Name} delete operation cause exception");
                throw ex;
            }
        }
    }
}
