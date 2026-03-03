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
           
            var password = DecodeBase64(passwordBase64);

           
            var userProfile = await _dbContext.UserProfiles.FirstOrDefaultAsync(p => p.ProfileName == "user");
            if (userProfile == null)
            {
                throw new Exception("Profile 'user' not found.");
            }

            
            var passwordHash = CreatePasswordHash(password);

           
            var newUser = new DataAccess.Models.User
            {
                UserName = username,
                Email = username,  
                PasswordHash = passwordHash,
                ProfileId = userProfile.ProfileId,
                CreatedAt = DateTime.UtcNow,
                StatusSession = false  
            };

          
            _dbContext.Users.Add(newUser);
            await _dbContext.SaveChangesAsync();

          
            return newUser.Email;
        }

        public async Task<bool> userStateSession(string username)
        {
         
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserName == username || u.Email == username);
            return user != null && user.StatusSession == true;
        }

       
        private string DecodeBase64(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

     
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
