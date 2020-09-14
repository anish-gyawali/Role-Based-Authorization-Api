using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AntsyProject_285.Features.Users
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
    }
}
