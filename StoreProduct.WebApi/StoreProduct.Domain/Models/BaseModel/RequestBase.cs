using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreProduct.Domain.Models.BaseModel
{
    public class RequestBase<T>
    {
        public T Data { get; set; }

        public bool Success { get; set; }

        public int StatusCode { get; set; }

        public dynamic Message { get; set; }
    }
    public class RequestBaseNoData
    {
        public bool Success { get; set; }

        public int StatusCode { get; set; }

        public dynamic Message { get; set; }
    }
}
