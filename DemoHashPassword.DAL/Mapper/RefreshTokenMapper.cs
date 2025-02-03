using DemoHashPassword.DL.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoHashPassword.DAL.Mapper
{
    public static class RefreshTokenMapper
    {
        public static RefreshToken ToRefreshToken(this IDataRecord reader)
        {
            return new RefreshToken
            {
                Id = (int)reader["Id"],
                UserId = (int)reader["UserId"],
                Token = (string)reader["Token"],
                ExpiredAt = (DateTime)reader["ExpiredAt"],
                Used = (bool)reader["Used"]
            };
        }
    }
}
