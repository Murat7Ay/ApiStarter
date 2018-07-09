using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WebApi.Factory;

namespace WebApi.Custom.Log
{
    public class LogRequestAndResponseHandler : DelegatingHandler
    {
        private static NLog.Logger logger = LoggerFactory.GetLoggerInstance();
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var guid = Guid.NewGuid();
            request.Headers.Add("X-Correlation-Id", guid.ToString());
            string requestBody = await request.Content.ReadAsStringAsync();
            logger.Info("Guid:{0}, Uri:{1} , Request:{2}", guid, request.RequestUri, requestBody);
            var result = await base.SendAsync(request, cancellationToken);

            if (result.Content != null)
            {
                var responseBody = await result.Content.ReadAsStringAsync();
                logger.Info("Guid:{0}, Uri:{1} , Response:{2}", guid, request.RequestUri, responseBody);
            }
            

            return result;
        }
    }
}