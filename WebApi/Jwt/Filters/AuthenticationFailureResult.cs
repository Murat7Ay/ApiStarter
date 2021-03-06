﻿using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using WebApi.Enums;
using WebApi.Factory;
using WebApi.Models;

namespace WebApi.Jwt.Filters
{
    public class AuthenticationFailureResult : IHttpActionResult
    {
        public AuthenticationFailureResult(string reasonPhrase, ResultEnum result,HttpRequestMessage request)
        {
            ReasonPhrase = reasonPhrase;
            Request = request;
            Result = result;
        }

        public ResultEnum Result { get; }

        public string ReasonPhrase { get; }

        public HttpRequestMessage Request { get; }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute());
        }

        private HttpResponseMessage Execute()
        {
            var response = Request.CreateResponse(HttpStatusCode.Unauthorized,new BaseResponse(Result));
            response.RequestMessage = Request;
            response.ReasonPhrase = ReasonPhrase;
            return response;
        }
    }
}