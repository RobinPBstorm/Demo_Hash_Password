using DemoHashPassword.DAL.Interfaces;
using DemoHashPassword.DAL.Repositories;
using DemoHashPassword.DL.Entities;
using DemoHashPasword.BLL.Intefaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoHashPasword.BLL.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _repository;
        public AuthService(IAuthRepository authRepository)
        {
            _repository = authRepository;
        }

        public User Login(string username, string password)
        {
            User? currentUser = _repository.GetOneByUsername(username);

            if (currentUser is null)
            {
                throw new Exception("Le login ou le mot de passe est incorrect");
            }

            string userPassword = _repository.GetPassword(currentUser.Username);
            if (password != userPassword)
            {
                throw new Exception("Le login ou le mot de passe est incorrect");
            }

            return currentUser;
        }

        public void Register(User user, string password)
        {
            _repository.Register(user, password);
        }
    }
}
