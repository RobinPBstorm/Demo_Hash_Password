using DemoHashPassword.DAL.Interfaces;
using DemoHashPassword.DAL.Mapper;
using DemoHashPassword.DL.Entities;
using Microsoft.Data.SqlClient;
using static DemoHashPassword.IL.Utils.ConnectionUtils;

namespace DemoHashPassword.DAL.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly SqlConnection _connection;
        public AuthRepository(SqlConnection sqlConnection)
        {
            _connection = sqlConnection;
        }

        public User? GetOneByUsername(string username)
        {
            using (SqlCommand cmd = _connection.CreateCommand())
            {
                cmd.CommandText =
                    "SELECT * " +
                    "FROM [User] " +
                    "WHERE [Username] = @username";
                cmd.Parameters.AddWithValue("username", username);

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

        public string GetPassword(string username)
        {
            using (SqlCommand cmd = _connection.CreateCommand())
            {
                cmd.CommandText =
                    "SELECT [password] " +
                    "FROM [User] " +
                    "WHERE [Username] = @username";
                cmd.Parameters.AddWithValue("username", username);

                string? password = null;

                OpenConnection(_connection);
                password = (string)cmd.ExecuteScalar();
                _connection.Close();

                return password;
            }
        }

        public void Register(User user, string password)
        {
            using (SqlCommand cmd = _connection.CreateCommand())
            {
                cmd.CommandText = "INSERT INTO [User] ([Username], [Password], [Name], [Firstname]) " +
                    "VALUES (@username, @password, @name, @firstname)";

                cmd.Parameters.AddWithValue("username", user.Username);
                cmd.Parameters.AddWithValue("password", password);
                cmd.Parameters.AddWithValue("name", user.Name);
                cmd.Parameters.AddWithValue("firstname", user.Firstname);
                
                try
                {
                    OpenConnection(_connection);
                    cmd.ExecuteNonQuery();
                    _connection.Close();
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
            }
        }
		public void ChangePassword(int id, string password)
		{
			using (SqlCommand cmd = _connection.CreateCommand())
			{
				cmd.CommandText = "UPDATE [User] " +
                    "SET [Password] = @password " +
					"WHERE [Id] = @id";

				cmd.Parameters.AddWithValue("password", password);
				cmd.Parameters.AddWithValue("id", id);

				try
				{
					OpenConnection(_connection);
					cmd.ExecuteNonQuery();
					_connection.Close();
				}
				catch (SqlException ex)
				{
					throw ex;
				}
			}
		}
	}
}
