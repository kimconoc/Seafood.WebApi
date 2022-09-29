using StoreProduct.Domain.Models.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreProduct.Domain.Models.DataAccessModel
{
    public class Product : VBaseModel
    {
        public string CategoryCode { get; set; }
        public string RegionId { get; set; }
        public Guid ImgeProdCode { get; set; }
        public string Name { get; set; }
        public string DescPromotion { get; set; }
        public string Description { get; set; }
        public string Outstanding { get; set; }
        public int Price { get; set; }
        public int PriceSale { get; set; }
        public float Amount { get; set; }
        public string Note { get; set; }
    }
}
