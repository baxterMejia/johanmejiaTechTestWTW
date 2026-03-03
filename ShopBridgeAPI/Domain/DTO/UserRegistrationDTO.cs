using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class UserRegistrationDTO
    {
        public string Username { get; set; }
        public string Pass { get; set; }
        public string PassConfirm { get; set; }
    }
}
