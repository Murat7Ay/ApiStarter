using System.Collections.Generic;
using System.Linq;
using System.Web.Http.ExceptionHandling;
using WebApi.Factory;

namespace WebApi.Custom.Log
{
    public class UnhandledExceptionLogger : ExceptionLogger
    {
        private static NLog.Logger logger = LoggerFactory.GetLoggerInstance();
        private IEnumerable<string> _correlationGuid;
        public override void Log(ExceptionLoggerContext context)
        {
            context.Request.Headers.TryGetValues("X-Correlation-Id", out _correlationGuid);

            var exceptionMessage = context.Exception.InnerException == null ? context.Exception.Message : context.Exception.InnerException.Message;

            logger.Error(exceptionMessage, "Guid:{0}, Uri:{1} ", _correlationGuid.First(), context.Request.RequestUri);
        }
    }
}