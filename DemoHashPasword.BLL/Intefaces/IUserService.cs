using DemoHashPassword.DL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoHashPasword.BLL.Intefaces
{
    public interface IUserService
    {
        IEnumerable<User> GetAll();
        User GetOneById(int id);
    }
}
