using DataAccess;
using DataAccess.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Test.Models;

namespace Test.Services
{
    public class UserService : IUserService
    {
        private readonly AppSettings _appSettings;
        private readonly ApiContext _db;

        public UserService(IOptions<AppSettings> appsettings, ApiContext db)
        {
            _appSettings = appsettings.Value;
            _db = db;
        }

        public async Task<AuthenticateResponse> Authenticate(string username, string password)
        {
            User user = await Task.Run(() => _db.User.SingleOrDefault(x => x.Username == username && x.Password == password));

            // return null if user not found
            if (user == null)
                return null;

            // authentication successful so return user details without password
            return new AuthenticateResponse(user, generateJwtToken(user));
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await Task.Run(() => _db.User.WithoutPasswords());
        }

        private string generateJwtToken(User user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
