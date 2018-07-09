using System.Collections.Generic;
using WebApi.Enums;
using WebApi.Jwt.CheckUser;
using WebApi.Models;

namespace WebApi.Jwt
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using Microsoft.IdentityModel.Tokens;

    namespace WebApi.Jwt
    {
        public static class JwtManager
        {
            private const string Secret = "db3OIsj+BXE9NZDy0t8W3TcNekrF+2d/1sFnWG4HnV8TZY30iTOdtVWJG8abWvB1GlOgJuQZdcF2Luqm/hccMw==";

            public static ResponseAuth GenerateToken(AuthRequest model, int expireMinutes = 120)
            {
                var symmetricKey = Convert.FromBase64String(Secret);
                var tokenHandler = new JwtSecurityTokenHandler();

                var now = DateTime.UtcNow;
                List<Claim> claims = new List<Claim> {new Claim(ClaimTypes.Name, model.Username)};

                var user = new AuthUser();

                var userModel = user.GetUser(model.Username);

                foreach (var role in userModel.UserRoles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),

                    Expires = now.AddMinutes(Convert.ToInt32(expireMinutes)),

                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256Signature)
                };

                var stoken = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(stoken);

                return new ResponseAuth(ResultEnum.AuthUser,token,tokenDescriptor.Expires.Value);
            }

            public static ClaimsPrincipal GetPrincipal(string token)
            {
                try
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

                    if (jwtToken == null)
                        return null;

                    var symmetricKey = Convert.FromBase64String(Secret);

                    var validationParameters = new TokenValidationParameters()
                    {
                        RequireExpirationTime = true,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        IssuerSigningKey = new SymmetricSecurityKey(symmetricKey)
                    };

                    SecurityToken securityToken;
                    var principal = tokenHandler.ValidateToken(token, validationParameters, out securityToken);

                    return principal;
                }

                catch (Exception)
                {
                    return null;
                }
            }
        }
    }
}