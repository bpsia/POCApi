using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace POCService.Helpers
{
    public class CustomPrincipal : IPrincipal
    {
        public CustomPrincipal(string Username)
        {
            this.Identity = new GenericIdentity(Username);
        }

        public CustomPrincipal(string Username, string[] roles) : this(Username)
        {
            this.roles = roles;
        }

        public IIdentity Identity { get; private set; }

        public bool IsInRole(string role)
        {
            if (roles.Any(r => role.Contains(r)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string[] roles { get; set; }
    }
}