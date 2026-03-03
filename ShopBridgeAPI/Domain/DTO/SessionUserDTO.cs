using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class SessionUserDTO
    {
        //Estos campos son un ejemplo para incorporar con la DB
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string AccessToken { get; set; }       
        public DateTime AccessTokenExpiry { get; set; }        
        public bool Status { get; set; }
    }
}
