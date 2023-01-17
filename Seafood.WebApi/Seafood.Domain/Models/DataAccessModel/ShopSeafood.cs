using Seafood.Domain.Models.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seafood.Domain.Models.DataAccessModel
{
    public class ShopSeafood : VBaseModel
    {
        public string CodeRegion { get; set; }
        public string CodeDistrict { get; set; }
        public string CodeWard { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int TypeAddress { get; set; }
        public string Mobile { get; set; }
        public string Code { get; set; }
        public string Note { get; set; }
    }
}
