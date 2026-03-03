using Microsoft.IdentityModel.Tokens;
using Repository.Interfaces;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Service.User
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public string GenerateToken(string username)
        {
            // Declaramos los claims que llevará el token. Los claims son declaraciones sobre una entidad
            // (normalmente, el usuario) y datos adicionales.
            var claims = new[]
            {
               new Claim(JwtRegisteredClaimNames.UniqueName, username),  // Claim para el nombre de usuario único.
               new Claim("Usuario", username)                 // Puedes añadir más claims según tu necesidad.
            };

            // Obtiene la clave secreta desde una variable de entorno para firmar el token.
            // Es crucial que esta clave esté segura y no sea expuesta.
            // Si la variable de entorno no existe, debería lanzar un error (no manejado aquí).
            var secretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY", EnvironmentVariableTarget.Machine);
            //var secretKey = "Pablo";
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            //Aqui se debe consultar en la DB cual es el tiempo del token, para efectos de ejemplo se usara un dummy     

            // Creamos el token usando la clase JwtSecurityToken.
            var jwt = new JwtSecurityToken(
                issuer: "APIGateway",          // La entidad que emite el token (tu aplicación)
                audience: "FrontTechMo",   // La entidad que debería recibir y usar el token (puede ser cualquiera en este caso)
                claims: claims,           // Los claims definidos anteriormente
                expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32("240")),  // El tiempo de expiración del token
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)  // Las credenciales para firmar el token
            );        


            // Serializamos el token a un string y lo retornamos.
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
