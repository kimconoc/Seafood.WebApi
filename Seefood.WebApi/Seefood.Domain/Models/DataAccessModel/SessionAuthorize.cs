using Seefood.Domain.Models.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seefood.Domain.Models.DataAccessModel
{
    public class SessionAuthorize : VBaseModel
    {
        public string Username { get; set; }
        public string IpRequest { get; set; }
        public string SessionId { get; set; }
        public string Session { get; set; }
    }
}
