using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Security.Principal;
namespace GB.SUTEL.UI.Models
{
    public class CustomPrincipal : IPrincipal
    {
        public IIdentity Identity { get; private set; }
        public bool IsInRole(int role)
        {            
            if(roles.ToList().IndexOf(role) != -1)
            //if (roles.Any(r => role.(r)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public CustomPrincipal(string Username)
        {
            this.Identity = new GenericIdentity(Username);
        }

        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int[] roles { get; set; }


        public bool IsInRole(string role)
        {
            throw new NotImplementedException();
        }
    }

    public class CustomPrincipalSerializeModel
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public List<string> roles { get; set; }
    }
}