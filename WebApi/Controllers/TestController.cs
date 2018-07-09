using System;
using System.Collections.Generic;
using System.Web.Http;
using WebApi.Custom.Compression;
using WebApi.Enums;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TestController : ApiController
    {

        public IHttpActionResult Get(int id)
        {
            throw new Exception("Deneme");
        }

        [DeflateCompression]
        public IHttpActionResult Get(string id, string text)
        {
            var dateList = new List<DateTime>();
            
            var response = new TestResponse(ResultEnum.OperationSucces, dateList);

            return Ok(response);
        }
    }
}
