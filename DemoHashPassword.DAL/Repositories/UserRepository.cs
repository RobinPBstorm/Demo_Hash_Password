using DemoHashPassword.DAL.Interfaces;
using DemoHashPassword.DL.Entities;
using static DemoHashPassword.IL.Utils.ConnectionUtils;
using Microsoft.Data.SqlClient;
using DemoHashPassword.DAL.Mapper;

namespace DemoHashPassword.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SqlConnection _connection;
        public UserRepository(SqlConnection sqlConnection)
        {
            _connection = sqlConnection;
        }
        public IEnumerable<User> GetAll()
        {
            using (SqlCommand cmd = _connection.CreateCommand())
            {
                cmd.CommandText =
                    "SELECT * " +
                    "FROM [User]";
                List<User> users = new List<User>();
                OpenConnection(_connection);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    users.Add(reader.ToUser());
                }
                _connection.Close();
                return users;
            }
        }

        public User? GetOneById(int id)
        {
            using (SqlCommand cmd = _connection.CreateCommand())
            {
                cmd.CommandText =
                    "SELECT * " +
                    "FROM [User] " +
                    "WHERE Id = @id";
                cmd.Parameters.AddWithValue("id", id);

                User? user = null;
                OpenConnection(_connection);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    user = reader.ToUser();
                }
                _connection.Close();
                return user;
            }
        }
    }
}
