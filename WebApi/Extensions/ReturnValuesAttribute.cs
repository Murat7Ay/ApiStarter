using System;

namespace WebApi.Extensions
{
    public class ReturnValuesAttribute : Attribute
    {
        internal ReturnValuesAttribute(bool success, int code, string message, string info = null)
        {
            Success = success;
            Code = code;
            Message = message;
            Info = info;
        }
        public bool Success { get; }
        public int Code { get; }
        public string Message { get; }
        public string Info { get; }
    }
}