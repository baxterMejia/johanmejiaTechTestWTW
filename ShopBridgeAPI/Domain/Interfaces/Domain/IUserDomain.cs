using Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Domain
{
    public interface IUserDomain
    {      
        /// <summary>
        /// Metodo para registrar usuarios
        /// </summary>
        /// <param name="credentials"></param>
        /// <returns></returns>
        Task<string> RegisterUser(UserRegistrationDTO credentials);

        /// <summary>
        /// Metodo para retornar informacion de los modulos y los usuarios
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<string> GetData(string email);
    }
}
