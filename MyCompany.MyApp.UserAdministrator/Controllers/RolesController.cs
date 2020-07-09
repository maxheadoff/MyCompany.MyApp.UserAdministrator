using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyCompany.MyApp.UserAdministrator.Dtos;
using MyCompany.MyApp.UserAdministrator.Repository;

namespace MyCompany.MyApp.UserAdministrator.Controllers
{
    /// <summary>
    /// roles rest-controller
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly IRoleRepo _repo = null;
        private readonly ILogger<UsersController> _logger;
        private readonly IMapper _mapper;

         public RolesController(IRoleRepo repo, ILogger<UsersController> logger, IMapper mapper) : base()
        {
            _repo = repo;
            _logger = logger;
            _mapper = mapper;
        }
        // GET: api/<controller>
        [HttpGet]
       // [Authorize()]
        public async Task<ActionResult<IEnumerable<RoleReadDto>>> Get()
        {
            _logger.LogTrace("get roles");
            return Ok(_mapper.Map<IEnumerable<RoleReadDto>>(await _repo.GetRoles()));
        }


    }
}