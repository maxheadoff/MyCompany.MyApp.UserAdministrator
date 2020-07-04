using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyCompany.MyApp.UserAdministrator.Dtos;
using MyCompany.MyApp.UserAdministrator.Models;
using MyCompany.MyApp.UserAdministrator.Repository;

namespace MyCompany.MyApp.UserAdministrator.Controllers
{
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
        public async Task<ActionResult<IEnumerable<UserReadDto>>> Get()
        {
            _logger.LogTrace("get users");
            return Ok(_mapper.Map<IEnumerable<UserReadDto>>(await _repo.GetUsers()));
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserReadDto>> GetById(int id)
        {
            var user = await _repo.GetUserById(id);
            if (user == null)
                return NotFound();
            return Ok(_mapper.Map<UserReadDto>(user));
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<ActionResult<UserReadDto>> Post([FromBody]UserCreateDto dto)
        {
            var model = _mapper.Map<User>(dto);
            await _repo.CreateUser(model);
            await _repo.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = model.Id }, _mapper.Map<UserReadDto>(model));
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, UserUpdateDto dto)
        {
            var model = await _repo.GetUserById(id);
            if (model == null)
                return NotFound();
            _mapper.Map(dto, model);
            _repo.UpdateUser(model);
            await _repo.SaveChanges();
            return NoContent();
        }

        // DELETE api/<controller>/5
        [Authorize(Roles="Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var model = await _repo.GetUserById(id);
            if (model == null)
                return NotFound();
            _repo.DeleteUser(model);
            await _repo.SaveChanges();
            return NoContent();
        }
    }
}
