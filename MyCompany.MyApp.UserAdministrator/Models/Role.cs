using System.Collections.Generic;

namespace MyCompany.MyApp.UserAdministrator.Models
{
    public class Role:ModelBase
    {
       public ICollection<UserRole> UserRoles { get; set; }
    }
}