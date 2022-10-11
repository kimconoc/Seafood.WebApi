using Seefood.Domain.Models.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seefood.Domain.Models.DataAccessModel
{
    public class ShopSeafood : VBaseModel
    {
        public string RegionDistrictCode { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Code { get; set; }
        public string Note { get; set; }
    }
}
