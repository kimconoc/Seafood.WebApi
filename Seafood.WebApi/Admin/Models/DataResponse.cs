using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Amin.Models
{
    public class StatusResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }

    }

    public class DataResponse<T>
    {
        public T Data { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }
        
    }
}