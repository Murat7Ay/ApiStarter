using System.Web.Http;
using WebApi.Jwt.CheckUser;
using WebApi.Jwt.WebApi.Jwt;
using WebApi.Models;

namespace WebApi.Controllers
{
    [AllowAnonymous]
    public class AuthController : ApiController
    {
        [HttpPost]
        public IHttpActionResult Post([FromBody]AuthRequest authModel)
        {
            var user = new AuthUser();

            if (user.GetUser(authModel.Username, authModel.Password) == null) return Unauthorized();


            return Ok(JwtManager.GenerateToken(authModel));
        }
    }
}
