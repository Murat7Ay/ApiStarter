using WebApi.Custom.JilJson;
using WebApi.Enums;
using WebApi.Models;

namespace WebApi.Factory
{
    public static class ExceptionFactory
    {
        public static string GetExceptionText(string exceptionMessage=null)
        {
            return JilHelper.Serialize(new GlobalExceptionModel(ResultEnum.Exception,exceptionMessage));
        }
    }
}