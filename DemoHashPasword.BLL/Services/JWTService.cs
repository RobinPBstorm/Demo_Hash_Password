using DemoHashPassword.DL.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace DemoHashPasword.BLL.Services
{
    public class JWTService
    {
        private IConfiguration _Configuration;
        public JWTService(IConfiguration configuration)
        {
            _Configuration = configuration;
        }

        public string GenerateToken(User user)
        {
            // Création de la signature du Jwt sur base d'une clef secret
            byte[] key = Encoding.UTF8.GetBytes(_Configuration["JwtOptions:Secret"]);
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);

            // Création d'un objet de sécurité avec les données necessaire au token
            IEnumerable<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.IsAdmin ? "Admin": "user")
            };


            // Création du token
            // Alternative: Utiliser le "SecurityTokenDescriptor"
            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _Configuration["JwtOptions:Issuer"],
                audience: _Configuration["JwtOptions:Audience"],
                expires: DateTime.Now.AddSeconds(_Configuration["JwtOptions:Expiration"] is null ? 1 : double.Parse(_Configuration["JwtOptions:Expiration"])),
                claims: claims,
                signingCredentials: credentials
            );

            // Génération sous forme de chaine de caractere 
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}
