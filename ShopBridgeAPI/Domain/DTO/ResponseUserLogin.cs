using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class ResponseUserLogin
    {
        public string userName { get; set;}
        public string token { get; set;}
        public string profile { get; set;}
        public List<Modules> modules { get; set;}
    }

    public class Modules 
    {
        public string moduleName { get; set; }
        public string Route { get; set; }
    }
}
