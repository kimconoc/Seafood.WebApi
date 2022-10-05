using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seefood.Domain.Models.ParameterModel
{
    public class CreateAccountParameter
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string NumberPhone { get; set; }
        public string Password { get; set; }
    }
}
