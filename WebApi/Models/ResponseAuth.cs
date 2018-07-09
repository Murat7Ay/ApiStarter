using System;
using WebApi.Enums;

namespace WebApi.Models
{
    public class ResponseAuth:BaseResponse
    {
        public ResponseAuth(ResultEnum result,string token,DateTime expiresAt):base(result)
        {
            ExpiresAt = expiresAt;
            Token = token;
        }
        public DateTime ExpiresAt { get; set; }
        public string Token { get; set; }
    }
}