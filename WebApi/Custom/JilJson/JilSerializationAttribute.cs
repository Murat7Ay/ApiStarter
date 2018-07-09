using System;
using System.Web.Http.Controllers;

namespace WebApi.Custom.JilJson
{
    public sealed class JilSerializationAttribute : Attribute, IControllerConfiguration
    {
        public void Initialize(HttpControllerSettings controllerSettings, HttpControllerDescriptor controllerDescriptor)
        {
            controllerSettings.Formatters.Insert(0, new JilMediaTypeFormatter());
        }
    }
}