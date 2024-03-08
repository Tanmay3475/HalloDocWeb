using HalloDoc.Models;
using HalloDoc.Repository.IRepository;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Repository
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration configuration;
        public JwtService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string GenerateJwtToken(LoggedInPersonViewModel loggedInPersonViewModel)
        {
            
            var claims = new List<Claim>
            {
            new Claim("Role",loggedInPersonViewModel.Role),
           new Claim("Name",loggedInPersonViewModel.UserName),
            new Claim("userId",loggedInPersonViewModel.AspnetId)
        };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(20);
            var token = new JwtSecurityToken(
                configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
                claims,
                expires: expires,
                signingCredentials: creds
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        //public string GenerateJwtToken(User user)
        //{
        //    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        //    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        //    // Define claims
        //    var claims = new[]
        //    {
        //    new Claim(ClaimTypes.NameIdentifier, user.Id),
        //    new Claim(ClaimTypes.Email, user.Email),
        //    new Claim(ClaimTypes.Role, user.Role),
        //};

        //    // Create the JwtSecurityToken object
        //    var token = new JwtSecurityToken(
        //        issuer: _issuer,
        //        audience: _audience,
        //        claims: claims,
        //        expires: DateTime.UtcNow.AddMinutes(30), // Set token expiration time (e.g., 30 minutes)
        //        signingCredentials: credentials
        //    );

        //    // Use JwtSecurityTokenHandler to write the token
        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    return tokenHandler.WriteToken(token);
        //}

        public bool ValidateToken(string token, out JwtSecurityToken jwtSecurityToken)
        {
            jwtSecurityToken = null;
            if (token == null)
                return false;
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);
                jwtSecurityToken = (JwtSecurityToken)validatedToken;
                if (jwtSecurityToken != null)
                    return true;
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
