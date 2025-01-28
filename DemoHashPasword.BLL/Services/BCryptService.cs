using BCrypt.Net;
using DemoHashPasword.BLL.Intefaces;
using Microsoft.Extensions.Configuration;

namespace DemoHashPasword.BLL.Services
{
    public class BCryptService: IHashService
    {
        private readonly IConfiguration _Configuration;
        public BCryptService(IConfiguration configuration)
        {
            _Configuration = configuration;
        }
        public bool Verify(string password, string hashPassword)
        {
            HashType hashType;
            if (!Enum.TryParse<HashType>(_Configuration["BCryptConfig:hashType"],out hashType))
            {
                hashType = HashType.SHA384;
            }

            return BCrypt.Net.BCrypt.Verify(password, hashPassword);
        }

        public string HashPassword(string password)
        {
            string saltConfig = _Configuration["BCryptConfig:salt"];

            if (saltConfig is not null)
            {
                return BCrypt.Net.BCrypt.HashPassword(password, saltConfig);
            }

            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}
