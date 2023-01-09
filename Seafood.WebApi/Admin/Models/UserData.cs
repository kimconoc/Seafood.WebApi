using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Amin.Models
{
    public class UserData
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public List<string> Roles { get; set; }
        public string Avarta { get; set; }
        public DateTime Birthday { get; set; }
        public int Sex { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
    }
}