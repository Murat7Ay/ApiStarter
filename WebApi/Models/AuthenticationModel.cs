using System.Collections.Generic;

namespace WebApi.Models
{
    public class AuthenticationModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public List<string> UserRoles { get; set; }
    }
}