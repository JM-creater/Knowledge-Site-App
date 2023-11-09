using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSiteApp.Models.Dto
{
    public class LoginUserDto
    {
        public string UsernameOrEmail { get; set;}
        public string Password { get; set; }
    }
}
