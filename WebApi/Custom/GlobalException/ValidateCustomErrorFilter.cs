using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using WebApi.Factory;

namespace WebApi.Custom.GlobalException
{
    public class ValidateCustomErrorFilter : ActionFilterAttribute
    {
        private static NLog.Logger logger = LoggerFactory.GetLoggerInstance();

        private IEnumerable<string> _correlationGuid;

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext.ModelState.IsValid == false)
            {
                var errorList = string.Join(",", (from item in actionContext.ModelState
                    where item.Value.Errors.Any()
                    select item.Value.Errors[0].ErrorMessage).ToList());

                actionContext.Request.Headers.TryGetValues("X-Correlation-Id", out _correlationGuid);

                logger.Error("Messasge: {0}, Guid:{1}, Uri:{2} ", errorList, _correlationGuid.First(), actionContext.Request.RequestUri);

                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(ExceptionFactory.GetExceptionText(errorList), Encoding.UTF8, "application/json"),
                    RequestMessage = actionContext.Request
                };

                actionContext.Response = response;
            }
        }
    }
}