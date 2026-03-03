using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IUserService
    {       

        /// <summary>
        /// Generacion de token
        /// </summary>
        /// <param name="username">Generacion de Token</param>
        /// <returns></returns>
        string GenerateToken(string username);
     
    }
}
