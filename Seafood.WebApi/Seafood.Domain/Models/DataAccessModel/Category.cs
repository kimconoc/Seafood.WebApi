using Seafood.Domain.Models.BaseModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seafood.Domain.Models.DataAccessModel
{
    [Table("Categorys")]
    public class Category : VBaseModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }
        public string Code { get; set; }
        public string Icon { get; set; }
    }
}
