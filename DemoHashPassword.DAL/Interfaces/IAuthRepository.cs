using DemoHashPassword.DL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoHashPassword.DAL.Interfaces
{
    public interface IAuthRepository
    {
        User? GetOneByUsername(string username);
        void Register(User user, string password);
        string GetPassword(string username);
		void ChangePassword(int id, string password);
	}
}
