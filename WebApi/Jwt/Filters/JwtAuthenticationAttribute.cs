using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using WebApi.Enums;
using WebApi.Jwt.CheckUser;
using WebApi.Jwt.WebApi.Jwt;

namespace WebApi.Jwt.Filters
{
    public class JwtAuthenticationAttribute : Attribute, IAuthenticationFilter
    {
        public string Realm { get; set; }
        public string Roles { get; set; }
        public bool AllowMultiple => false;

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            var request = context.Request;
            var authorization = request.Headers.Authorization;

            if (authorization == null || authorization.Scheme != "Bearer")
            {
                context.ErrorResult = new AuthenticationFailureResult("Missing Bear", ResultEnum.MissingBearerToken, request);
                return;
            }

            if (string.IsNullOrEmpty(authorization.Parameter))
            {
                context.ErrorResult = new AuthenticationFailureResult("Missing Jwt Token", ResultEnum.MissingJwtToken, request);
                return;
            }

            var token = authorization.Parameter;
            var principal = await AuthenticateJwtToken(token);

            if (principal == null)
            {
                context.ErrorResult = new AuthenticationFailureResult("Invalid token", ResultEnum.InvalidToken, request);
            }
            else
                context.Principal = principal;
        }



        private static bool ValidateToken(string token, string roles, out string username, out List<string> userRoles)
        {
            username = null;
            userRoles = null;
            var contextRoles = roles.Split(',');
            var simplePrinciple = JwtManager.GetPrincipal(token);
            var identity = simplePrinciple?.Identity as ClaimsIdentity;

            if (identity == null)
                return false;

            if (!identity.IsAuthenticated)
                return false;

            var usernameClaim = identity.FindFirst(ClaimTypes.Name);
            username = usernameClaim?.Value;

            if (string.IsNullOrEmpty(username))
                return false;

            var user = new AuthUser();
            var userModel = user.GetUser(usernameClaim.Value);

            if (userModel == null)
                return false;

            if (!userModel.UserRoles.Any(contextRoles.Contains))
                return false;

            userRoles = userModel.UserRoles;

            return true;
        }

        protected Task<IPrincipal> AuthenticateJwtToken(string token)
        {
            string username;
            List<string> userRoles;
            if (ValidateToken(token, Roles, out username, out userRoles))
            {
                // based on username to get more information from database in order to build local identity
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username)
                    // Add more claims if needed: Roles, ...
                };

                foreach (var userRole in userRoles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var identity = new ClaimsIdentity(claims, "Jwt");
                IPrincipal user = new ClaimsPrincipal(identity);

                return Task.FromResult(user);
            }

            return Task.FromResult<IPrincipal>(null);
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            Challenge(context);
            return Task.FromResult(0);
        }

        private void Challenge(HttpAuthenticationChallengeContext context)
        {
            string parameter = null;

            if (!string.IsNullOrEmpty(Realm))
                parameter = "realm=\"" + Realm + "\"";

            context.ChallengeWith("Bearer", parameter);
        }
    }
}