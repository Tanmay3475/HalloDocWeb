using HalloDoc.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Repository.IRepository
{
    public interface IJwtService
    {
        string GenerateJwtToken(LoggedInPersonViewModel loggedInPersonViewModel);
        bool ValidateToken(string token,out JwtSecurityToken jwtSecurityToken);
    }
}
