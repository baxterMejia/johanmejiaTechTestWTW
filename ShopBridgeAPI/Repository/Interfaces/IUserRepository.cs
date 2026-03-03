using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public  interface IUserRepository
    {
        /// <summary>
        /// Metodo que valida el estado de la sesion del usuario.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        Task<bool> userStateSession(string username);

        /// <summary>
        /// Metodo de creacion de Usuario
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<string> userCreate(string username, string password);

        /// <summary>
        /// Metodo que retorna informaciond de los modulos y los usuarios
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<string> GetData(string email);
    }
}
