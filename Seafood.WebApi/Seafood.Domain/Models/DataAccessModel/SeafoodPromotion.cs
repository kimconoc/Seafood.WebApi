using Seafood.Domain.Models.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seafood.Domain.Models.DataAccessModel
{
    public class SeafoodPromotion : VBaseModel
    {
        public string ShopCode { get; set; }
        public string Content { get; set; }
        public string Note { get; set; }
    }
}
