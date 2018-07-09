using WebApi.Enums;
using WebApi.Extensions;

namespace WebApi.Models
{
    public class BaseResponse
    {
        public bool Success { get; set; }
        public int Code { get; set; }
        public string Message { get; set; }
        public string Info { get; set; }

        public BaseResponse(ResultEnum result)
        {
            var attributes = result.GetAttribute<ReturnValuesAttribute>();
            Success = attributes.Success;
            Code = attributes.Code;
            Message = attributes.Message;
            Info = attributes.Info;
        }

        public BaseResponse(bool success, int code, string message, string info = null)
        {
            Success = success;
            Code = code;
            Message = message;
            Info = info;
        }
    }
}