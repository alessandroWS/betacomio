using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace betacomio.Data
{
    public interface IAuthRepository
    {
        Task<ServiceResponse<int>> Register(UserCred userCred, string password, User user);
        Task<ServiceResponse<string>> Login(string username, string password);
        Task<bool> UserExists(string username);
    }
}