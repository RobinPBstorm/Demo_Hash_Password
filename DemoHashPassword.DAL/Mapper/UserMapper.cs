using DemoHashPassword.DL.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoHashPassword.DAL.Mapper
{
    public static class UserMapper
    {
        public static User ToUser(this IDataRecord reader)
        {
            return new User
            {
                Id = (int)reader["Id"],
                Username = (string)reader["Username"],
                Name = (string)reader["Name"],
                Firstname = (string)reader["Firstname"],
                IsAdmin = (bool)reader["IsAdmin"]
            };
        }
    }
}
