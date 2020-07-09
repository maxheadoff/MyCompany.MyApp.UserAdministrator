using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MyCompany.MyApp.UserAdministrator.Auth;
using MyCompany.MyApp.UserAdministrator.Models;
using MyCompany.MyApp.UserAdministrator.Repository;

namespace MyCompany.MyApp.UserAdministrator.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserRepo _repo = null;
        private readonly ILogger<UsersController> _logger;
        private readonly IMapper _mapper;

        public AccountController(IUserRepo repo, ILogger<UsersController> logger, IMapper mapper) :base()
        {
            _repo = repo;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost("/api/token")]
        public  async Task<ActionResult> Token(string username, string password)
        {
 /*           byte[] buffer=new byte[Request.ContentLength.Value];
            var bodyLength= await Request.Body.ReadAsync(buffer,0,Convert.ToInt32(Request.ContentLength.Value));
            if (bodyLength > 0)
            {
                var body = Encoding.UTF8.GetString(buffer);
                _logger.LogInformation(body);
            }
            */
            
            var identity = await GetIdentity(username, password);
            if (identity == null)
            {
                return BadRequest(new { errorText = "Invalid username or password." });
            }

            var now = DateTime.UtcNow;
            
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name
            };

            return Json(response);
        }

        private async Task<ClaimsIdentity> GetIdentity(string username, string password)
        {

            var user =  (await _repo.GetUsers()).FirstOrDefault(x => x.Login == username && x.Password == password);
            if (user == null)
                return null;
            user = await _repo.GetUserById(user.Id);
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login)
                };
                // Adds all roles
                foreach (var role in user.Roles)
                    claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, role.Role.Name));
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }
            return null;
        }
    }
}