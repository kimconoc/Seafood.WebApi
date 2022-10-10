using Seefood.Domain.Models.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seefood.Domain.Models.DataAccessModel
{
    public class Region : VBaseModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }
        public string Code { get; set; }

    }
}
