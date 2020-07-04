using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCompany.MyApp.UserAdministrator.Dtos
{
    public abstract class DtoBase
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
