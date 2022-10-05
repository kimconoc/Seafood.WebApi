using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seefood.Domain.Models.BaseModel
{
    public interface IEntity
    {
        Guid Id { get; set; }
        bool IsDeleted { get; set; }
        string DeletedBy { get; set; }
        Nullable<DateTime> DeletedAt { get; set; }
        string CreatedBy { get; set; }
        Nullable<DateTime> CreatedAt { get; set; }
        string UpdatedBy { get; set; }
        Nullable<DateTime> UpdatedAt { get; set; }
    }
}
