using DataAccess.Models;
using Domain.DTO;
using Domain.Interfaces.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace BaseOWASP.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserDomain _userDomain;
        public UserController(IUserDomain userDomain)
        {
            _userDomain = userDomain;
        }

        /// <summary>
        /// Metodo de registro de usuarios
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost("RegisterUser")]
        public async Task<ActionResult<string>> RegisterUser([FromBody] UserRegistrationDTO requestDto)
        {
            //Autenticacion de Credenciales de Usuario
            string response = await _userDomain.RegisterUser(requestDto);
            if (response.Contains("Incorrecto"))
            {
                return Unauthorized("Credenciales incorrectas.");
            }
            else
            {
                return Ok(new { token = response }); // Devuelve el token dentro de un objeto JSON
            }
        }


        /// <summary>
        /// Login de la Aplicacion
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        //[Authorize]
        //[HttpPost("GetData")]
        //public async Task<ActionResult<string>> GetData([FromBody] UserCredentialsDTO requestDto)
        //{
        //    //Autenticacion de Credenciales de Usuario
        //    //string response = await _userDomain.Authenticate(requestDto);
        //    if (response.Contains("Incorrecto"))
        //    {
        //        return Unauthorized("Credenciales incorrectas.");
        //    }
        //    else
        //    {

        //        return Ok(new { token = response }); // Devuelve el token dentro de un objeto JSON
        //    }
        //}
    }
}
