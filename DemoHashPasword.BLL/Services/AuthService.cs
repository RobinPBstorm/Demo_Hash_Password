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
        private readonly IUserRepository _userRepository;
		private readonly IHashService _hashService;
        private readonly IRefreshTokenService _refreshTokenService;
        public AuthService(IAuthRepository authRepository, 
                            IHashService hashService,
                            IRefreshTokenService refreshTokenService,
                            IUserRepository userRepository)
        {
            _repository = authRepository;
            _hashService = hashService;
            _refreshTokenService = refreshTokenService;
            _userRepository = userRepository;
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
            if (_repository.GetOneByUsername(user.Username) is not null)
            {
                throw new Exception("Ce Username est déjà utilisé");
            }

			string hashPassword = _hashService.HashPassword(password);
			_repository.Register(user, hashPassword);
		}

		public void ChangePassword(int id, string oldPassword, string newPassword)
		{
            User user = _userRepository.GetOneById(id);
            string passwordDb = _repository.GetPassword(user.Username);
			if (!_hashService.Verify(oldPassword, passwordDb))
			{
				throw new Exception("Vous n'avez pas donner le bon mot de passe");
			}

            _repository.ChangePassword(id, _hashService.HashPassword(newPassword));
		}
	}
}
