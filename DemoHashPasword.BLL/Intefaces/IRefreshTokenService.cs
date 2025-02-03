using Azure.Core;
using DemoHashPassword.DL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoHashPasword.BLL.Intefaces
{
    public interface IRefreshTokenService
    {
        Tokens GenerateRefreshToken(string username);
        Tokens GetRefreshedToken(string token, int id);
    }
}
