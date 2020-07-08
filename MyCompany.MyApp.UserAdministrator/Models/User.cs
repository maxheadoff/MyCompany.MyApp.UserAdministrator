using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyCompany.MyApp.UserAdministrator.Models
{
    public class User:ModelBase
    {
        [Required]
        [MaxLength(255)]
        public string Login { get; set; }
        [MaxLength(255)]
        public string Email { get; set; }
        [MaxLength(255)]
        public string Password { get; set; }
        public ICollection<UserRole> Roles { get; set; }
    }
}
