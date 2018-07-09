using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using WebApi.Custom.GlobalException;
using WebApi.Custom.JilJson;
using WebApi.Custom.Log;

namespace WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.ConfigureJSONFormatter();
            //
            config.Filters.Add(new ValidateCustomErrorFilter());
            config.Filters.Add(new AuthorizeAttribute());
            config.Services.Replace(typeof(IExceptionHandler), new GlobalExceptionHandler());
            config.Services.Replace(typeof(IExceptionLogger), new UnhandledExceptionLogger());
            config.MessageHandlers.Add(new LogRequestAndResponseHandler());
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }

        private static void ConfigureJSONFormatter(this HttpConfiguration config)
        {

            // Remove all formatter and use Jil
            config.Formatters.Clear();
            config.Formatters.Insert(0, new JilMediaTypeFormatter());
        }
    }
}
