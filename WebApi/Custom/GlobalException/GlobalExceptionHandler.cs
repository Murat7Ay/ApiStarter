using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using WebApi.Factory;


namespace WebApi.Custom.GlobalException
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        //Custom error mesajı
        private readonly string _exceptionJson = ExceptionFactory.GetExceptionText();

        public Task HandleAsync(ExceptionHandlerContext context, CancellationToken cancellationToken)
        {
            context.Result = new JsonPlainErrorResult
            {
                Request = context.ExceptionContext.Request,
                Content = _exceptionJson
            };

            return Task.FromResult(0);
        }

        private class JsonPlainErrorResult : IHttpActionResult
        {
            public HttpRequestMessage Request { private get; set; }

            public string Content { private get; set; }

            public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
            {
                HttpResponseMessage response =
                    new HttpResponseMessage(HttpStatusCode.InternalServerError);
                response.Content = new StringContent(Content, Encoding.UTF8, "application/json");
                response.RequestMessage = Request;
                return Task.FromResult(response);
            }
        }
    }
}