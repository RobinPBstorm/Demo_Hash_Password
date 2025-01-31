using DemoHashPassword.DL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoHashPasword.BLL.Intefaces
{
    public interface IAuthService
    {
        void Register(User user, string password);
        string Login(string username, string password);
    }
}
