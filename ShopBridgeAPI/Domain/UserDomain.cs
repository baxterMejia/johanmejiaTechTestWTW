using Domain.DTO;
using Domain.Interfaces.Domain;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class UserDomain : IUserDomain
    {
        private readonly IUserService _userService;      
        public UserDomain(IUserService userService)
        {
            _userService = userService;
        }

        public Task<string> GetData(string email)
        {
            throw new NotImplementedException();
        }

        public Task<string> RegisterUser(UserRegistrationDTO credentials)
        {
            throw new NotImplementedException();
        }
    }
}
