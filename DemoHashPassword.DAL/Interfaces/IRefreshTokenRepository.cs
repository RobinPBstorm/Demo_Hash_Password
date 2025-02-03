using DemoHashPassword.DL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoHashPassword.DAL.Interfaces
{
    public interface IRefreshTokenRepository
    {
        RefreshToken? GetRefreshToken(string token);

        void SaveRefreshToken(RefreshToken token);
        void UseRefreshToken(int id);
    }
}
