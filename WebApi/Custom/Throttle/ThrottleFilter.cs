using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using WebApi.Enums;
using WebApi.Factory;

namespace WebApi.Custom.Throttle
{
    public class ThrottleFilter : ActionFilterAttribute
    {
        private readonly Throttler _throttler;
        private readonly string _throttleGroup;

        public ThrottleFilter(
            int RequestLimit = 5,
            int TimeoutInSeconds = 10,
            [CallerMemberName] string throttleGroup = null)
        {
            _throttleGroup = throttleGroup;
            _throttler = new Throttler(throttleGroup, RequestLimit, TimeoutInSeconds);
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            SetIdentityAsThrottleGroup();

            if (_throttler.RequestShouldBeThrottled)
            {
                actionContext.Response = actionContext.Request.CreateResponse(
                    (HttpStatusCode)429, ExceptionFactory.GetExceptionText(ResultEnum.TooManyRequests));

                AddThrottleHeaders(actionContext.Response);
            }

            base.OnActionExecuting(actionContext);
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            SetIdentityAsThrottleGroup();
            if (actionExecutedContext.Exception == null) _throttler.IncrementRequestCount();
            AddThrottleHeaders(actionExecutedContext.Response);
            base.OnActionExecuted(actionExecutedContext);
        }

        private void SetIdentityAsThrottleGroup()
        {
            if (_throttleGroup == "identity")
                _throttler.ThrottleGroup = HttpContext.Current.User.Identity.Name;

            if (_throttleGroup == "ipaddress")
                _throttler.ThrottleGroup = HttpContext.Current.Request.UserHostAddress;
        }

        private void AddThrottleHeaders(HttpResponseMessage response)
        {
            if (response == null) return;

            foreach (var header in _throttler.GetRateLimitHeaders())
                response.Headers.Add(header.Key, header.Value);
        }
    }
}