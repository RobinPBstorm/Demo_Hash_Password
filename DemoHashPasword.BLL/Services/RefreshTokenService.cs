using DemoHashPassword.DAL.Interfaces;
using DemoHashPassword.DL.Entities;
using DemoHashPasword.BLL.Intefaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoHashPasword.BLL.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IRefreshTokenRepository _repository;
        private readonly IUserRepository _userRepository;
        private readonly IAuthRepository _authRepository;
        private readonly JWTService _jWTService;
        private IConfiguration _configuration;
        public RefreshTokenService (IConfiguration configuration,
                                    IRefreshTokenRepository refreshTokenRepository, 
                                    IUserRepository userRepository, 
                                    IAuthRepository authRepository,
                                    JWTService jWTService)
        {
            _configuration = configuration;
            _repository = refreshTokenRepository;
            _userRepository = userRepository;
            _authRepository = authRepository;
            _jWTService = jWTService;
        }

        public Tokens GenerateRefreshToken(string username)
        {
            User user = _authRepository.GetOneByUsername(username)!;
            string newRefreshToken;
            do
            {
                newRefreshToken = _jWTService.GenerateRefreshToken();
            }
            while (_repository.GetRefreshToken(newRefreshToken) is not null);

            RefreshToken tokenToSave = new RefreshToken(user.Id,
                                                        newRefreshToken,
                                                        DateTime.Now.AddHours(Double.Parse(_configuration["JwtOptions:RefreshTokenExpirationHours"])));

            _repository.SaveRefreshToken(tokenToSave);

            Tokens tokens = new Tokens(_jWTService.GenerateToken(user), newRefreshToken);

            return tokens;
        }

        public Tokens GetRefreshedToken(string token, int id)
        {
            RefreshToken? refreshToken = _repository.GetRefreshToken(token);
            if (refreshToken == null || refreshToken.ExpiredAt < DateTime.Now)
            {
                throw new Exception("Invalid or expired refresh token.");
            }

            _repository.UseRefreshToken(refreshToken.Id);

            if (refreshToken.UserId != id)
            {
                throw new Exception("Token Invalide");
            }

            User user = _userRepository.GetOneById(refreshToken.UserId);
            return GenerateRefreshToken(user.Username);

        }
    }
}
