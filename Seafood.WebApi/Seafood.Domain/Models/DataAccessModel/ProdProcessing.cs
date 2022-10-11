using Seafood.Domain.Models.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seafood.Domain.Models.DataAccessModel
{
    public class ProdProcessing : VBaseModel
    {
        public Guid? ProductId { get; set; }
        public string Name { get; set; }
        public int? Price { get; set; }
        public string Note { get; set; }
    }
}
