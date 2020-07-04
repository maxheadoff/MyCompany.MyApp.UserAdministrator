using System.Collections.Generic;

namespace MyCompany.MyApp.UserAdministrator.Dtos
{
    public class UserReadDto:DtoBase
    {
        public string Login { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public IEnumerable<RoleReadDto> Roles { get; set; }
    }
}
