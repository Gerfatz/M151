using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test.Models;

namespace Test.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAll();
        Task<AuthenticateResponse> Authenticate(string username, string password);
    }
}
