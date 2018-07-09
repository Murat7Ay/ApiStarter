using System.Collections.Generic;
using WebApi.Models;

namespace WebApi.Jwt.CheckUser
{
    public class AuthUser
    {

        AuthenticationModel user = new AuthenticationModel
        {
            IsActive = true,
            Password = "12345",
            UserName = "TestUser",
            UserRoles = new List<string> { "Admin" }
        };

        public AuthenticationModel GetUser(string userName, string password)
        {
            if (userName == user.UserName && password == user.Password)
            {
                return user;
            }

            return null;
        }


        public AuthenticationModel GetUser(string userName)
        {
            if (userName == user.UserName)
            {
                return user;
            }

            return null;
        }
    }
}