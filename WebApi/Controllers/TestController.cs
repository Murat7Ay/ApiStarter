using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WebApi.Custom.Compression;
using WebApi.Custom.Throttle;
using WebApi.Enums;
using WebApi.Jwt.Filters;
using WebApi.Models;

namespace WebApi.Controllers
{
    [JwtAuthentication(Roles = "Admin")]
    public class TestController : ApiController
    {
        public IHttpActionResult Get(int id)
        {
            throw new Exception("Deneme");
        }

        [ThrottleFilter(ThrottleGroup: "updates")]
        [DeflateCompression]
        public IHttpActionResult Get(string id, string text)
        {
            var dateList = new List<DateTime>();

            var response = new TestResponse(ResultEnum.OperationSucces, dateList);

            return Ok(response);
        }
    }

    public class Model
    {
        public List<string> TaskList { get; set; }
    }
}
