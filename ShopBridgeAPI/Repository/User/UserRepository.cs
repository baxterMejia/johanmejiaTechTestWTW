using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Repository.Interfaces;
using System;
using System.Threading.Tasks;

namespace Repository.User
{
    public class UserRepository : IUserRepository
    {
        private readonly ProductContext _dbContext;

        public UserRepository(ProductContext productDbContext)
        {
            _dbContext = productDbContext;
        }

        public async Task<string> userCreate(string username, string passwordBase64)
        {
            // Decodificar la contraseña de Base64
            var password = DecodeBase64(passwordBase64);

            // Obtener el ID del perfil "user"
            var userProfile = await _dbContext.UserProfiles.FirstOrDefaultAsync(p => p.ProfileName == "user");
            if (userProfile == null)
            {
                throw new Exception("Profile 'user' not found.");
            }

            // Crear el hash de la contraseña
            var passwordHash = CreatePasswordHash(password);

            // Crear el nuevo usuario
            var newUser = new DataAccess.Models.User
            {
                UserName = username,
                Email = username,  // Asumimos que el username es el email
                PasswordHash = passwordHash,
                ProfileId = userProfile.ProfileId,
                CreatedAt = DateTime.UtcNow,
                StatusSession = false  // Por defecto, la sesión no está activa
            };

            // Añadir el nuevo usuario a la base de datos
            _dbContext.Users.Add(newUser);
            await _dbContext.SaveChangesAsync();

            // Retornar el Email del usuario creado
            return newUser.Email;
        }

        public async Task<bool> userStateSession(string username)
        {
            // Realiza la validación en base de datos de que la sesión del usuario exista y se encuentre activa
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserName == username || u.Email == username);
            return user != null && user.StatusSession == true;
        }

        // Método auxiliar para decodificar Base64
        private string DecodeBase64(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        // Método auxiliar para crear el hash de la contraseña (puedes usar cualquier método de hashing seguro)
        private string CreatePasswordHash(string password)
        {            
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        public async Task<string> GetData(string email)
        {
            var user = await _dbContext.Users
                .Include(u => u.Profile)
                .ThenInclude(p => p.Modules)
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                throw new Exception("User not found.");
            }

            var profile = user.Profile;
            var modules = profile.Modules.Select(m => new
            {
                m.ModuleId,
                m.ModuleName,
                m.Description,
                m.Route
            }).ToList();

            var result = new
            {
                user.Email,
                ProfileName = profile.ProfileName,
                Modules = modules
            };

            return JsonConvert.SerializeObject(result);
        }
    }
}
