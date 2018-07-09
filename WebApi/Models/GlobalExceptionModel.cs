using WebApi.Enums;

namespace WebApi.Models
{
    public class GlobalExceptionModel:BaseResponse
    {
        public string Exception { get; set; }
        public GlobalExceptionModel(ResultEnum res,string exceptionMessage):base(res)
        {
            Exception = exceptionMessage;
        }
    }
}