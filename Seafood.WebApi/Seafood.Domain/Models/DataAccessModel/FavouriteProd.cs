using Seafood.Domain.Models.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seafood.Domain.Models.DataAccessModel
{
    public class FavouriteProd : VBaseModel
    {
        public Guid? UserId { get; set; }
        public Guid? ProductId { get; set; }
        public Guid? ProdBasketId { get; set; }
        public string IpRequest { get; set; }
        public string ClassName { get; set; }
    }
}
