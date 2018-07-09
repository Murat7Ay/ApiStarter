using System;
using System.Collections.Generic;
using WebApi.Enums;

namespace WebApi.Models
{
    public class TestResponse:BaseResponse
    {
        public List<DateTime> AvaibleDates { get; set; }
        public TestResponse(ResultEnum result,List<DateTime> dates):base(result)
        {
            AvaibleDates = dates;
        }
    }
}