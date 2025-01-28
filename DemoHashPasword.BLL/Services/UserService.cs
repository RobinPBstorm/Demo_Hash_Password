using DemoHashPassword.DAL.Interfaces;
using DemoHashPassword.DL.Entities;
using DemoHashPasword.BLL.Intefaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoHashPasword.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        public UserService(IUserRepository userRepository)
        {
            _repository = userRepository;
        }

        public IEnumerable<User> GetAll()
        {
            return _repository.GetAll();
        }

        public User GetOneById(int id)
        {
            User? user = _repository.GetOneById(id);
            if (user is null)
            {
                throw new Exception("User non trouvé");
            }
            return user;
        }
    }
}
