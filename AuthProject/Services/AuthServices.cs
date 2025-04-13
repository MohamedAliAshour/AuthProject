using AuthProject.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AuthProject.Services
{
    public class AuthServices : IAuth
    {
        private readonly IConfiguration _Configuration;

        public AuthServices(IConfiguration configuration)
        {
            _Configuration = configuration;
        }

        public string CreateToken(IdentityUser user, List<Claim> claims)  // Keep List<Claim>
        {
            var calims = new List<Claim>();

            calims.Add(new Claim(ClaimTypes.Email, user.Email));

            // Add the claims passed from the controller (already List<Claim>)
            calims.AddRange(claims);

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_Configuration["Jwt:Key"]));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _Configuration["Jwt:Issuer"],
                _Configuration["Jwt:Audience"],
                calims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
