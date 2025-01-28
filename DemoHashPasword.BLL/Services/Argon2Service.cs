using Isopoh.Cryptography.Argon2;
using Isopoh.Cryptography.SecureArray;
using DemoHashPasword.BLL.Intefaces;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace DemoHashPasword.BLL.Services
{
    public class Argon2Service: IHashService
    {
        private readonly IConfiguration _Configuration;
        public Argon2Service(IConfiguration configuration)
        {
            _Configuration = configuration;
        }
        public bool Verify(string password, string hashPassword)
        {
            return Argon2.Verify(hashPassword, password);
        }

        public string HashPassword(string password)
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] salt = Encoding.ASCII.GetBytes(_Configuration["Argon2Config:salt"]);

            // somewhere in the class definition:
            //   private static readonly RandomNumberGenerator Rng =
            //       System.Security.Cryptography.RandomNumberGenerator.Create();
            using (var rnd = RandomNumberGenerator.Create())
            {
                rnd.GetBytes(salt);
            }
            Argon2Config config = new Argon2Config
            {
                Type = Argon2Type.DataIndependentAddressing,
                Version = Argon2Version.Nineteen,
                TimeCost = int.Parse(_Configuration["Argon2Config:TimeCost"]),
                MemoryCost = int.Parse(_Configuration["Argon2Config:MemoryCost"]),
                Lanes = int.Parse(_Configuration["Argon2Config:Lanes"]),
                Threads = Environment.ProcessorCount, // higher than "Lanes" doesn't help (or hurt)
                Password = passwordBytes,
                Salt = salt, // >= 8 bytes if not null
                Secret = null, // from somewhere
                AssociatedData = null, // from somewhere
                HashLength = int.Parse(_Configuration["Argon2Config:HashLength"]), // >= 4
            };

            Argon2 _argon2A = new Argon2(config);
            string hashString;
            using (SecureArray<byte> hashA = _argon2A.Hash())
            {
                hashString = config.EncodeString(hashA.Buffer);
            }

            return hashString;
        }
    }
}
