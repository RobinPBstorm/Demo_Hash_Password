using DemoHashPassword.DAL.Interfaces;
using DemoHashPassword.DL.Entities;
using DemoHashPasword.BLL.Intefaces;
using BCrypt.Net;
using System.Reflection.Metadata.Ecma335;

namespace DemoHashPasword.BLL.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _repository;
        private readonly IHashService _hashService;
        private readonly IRefreshTokenService _refreshTokenService;
        public AuthService(IAuthRepository authRepository, 
                            IHashService hashService,
                            IRefreshTokenService refreshTokenService)
        {
            _repository = authRepository;
            _hashService = hashService;
            _refreshTokenService = refreshTokenService;
        }

        public Tokens Login(string username, string password)
        {
            User? currentUser = _repository.GetOneByUsername(username);

            if (currentUser is null)
            {
                throw new Exception("Le login ou le mot de passe est incorrect");
            }

            string userPassword = _repository.GetPassword(currentUser.Username);
            Console.WriteLine(_hashService.Verify(password, userPassword));
            if (!_hashService.Verify(password, userPassword))
            {
                throw new Exception("Le login ou le mot de passe est incorrect");
            }

            return _refreshTokenService.GenerateRefreshToken(username);
        }

        public void Register(User user, string password)
        {
            string hashPassword = _hashService.HashPassword(password);
            _repository.Register(user, hashPassword);
        }
    }
}
