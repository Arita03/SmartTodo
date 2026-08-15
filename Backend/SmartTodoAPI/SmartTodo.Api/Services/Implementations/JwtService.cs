using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SmartTodoAPI.Configurations;
using SmartTodoAPI.Models;
using SmartTodoAPI.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SmartTodoAPI.Services.Implementations
{
    public class JwtService: IJwtService
    {
        private readonly JwtSettings _jwtSettings;
        public JwtService(IOptions<JwtSettings> options)
        {
            _jwtSettings = options.Value;
        }
        public string GenerateToken(User user)
        {
            var claims = new List<Claim>() // to create the payload of jwt token,the claims become part of the JWT payload.
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };
            var keyBytes=Encoding.UTF8.GetBytes(_jwtSettings.SecretKey); // Convert the secret key to bytes required by SymmetricSecurityKey.
            var securityKey = new SymmetricSecurityKey(keyBytes);
            var credentials= new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256);// to create the signature of jwt token
            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims:claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryInMinutes),
                signingCredentials:credentials
                );
            var tokenHandler = new JwtSecurityTokenHandler(); //which converts that object into the actual string that the frontend receives:
            return tokenHandler.WriteToken(token);
        }
    }
}
