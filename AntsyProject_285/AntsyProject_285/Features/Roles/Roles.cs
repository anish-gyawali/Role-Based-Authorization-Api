using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AntsyProject_285.Features.Roles
{
    public class Roles
    {
        public const string Admin = nameof(Admin);
        public const string Customer = nameof(Customer);

        private static bool HasAnyRole(ClaimsPrincipal user, string target)
        {
            foreach(var role in target.Split(","))
            {
                if (user.IsInRole(role))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
