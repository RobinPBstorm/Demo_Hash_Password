using DemoHashPassword.DAL.Interfaces;
using DemoHashPassword.DAL.Mapper;
using DemoHashPassword.DL.Entities;
using Microsoft.Data.SqlClient;
using System.Data.Common;
using static DemoHashPassword.IL.Utils.ConnectionUtils;

namespace DemoHashPassword.DAL.Repositories
{
    public class RefreshTokenRepository: IRefreshTokenRepository
    {
        private readonly SqlConnection _connection;
        public RefreshTokenRepository(SqlConnection sqlConnection)
        {
            _connection = sqlConnection;
        }

        public RefreshToken? GetRefreshToken(string token)
        {
            using (SqlCommand cmd = _connection.CreateCommand())
            {
                cmd.CommandText =
                    "SELECT * " +
                    "FROM [RefreshToken] " +
                    "WHERE [Token] = @token AND [Used] = 0";
                cmd.Parameters.AddWithValue("token", token);

                RefreshToken? refreshToken = null;
                OpenConnection(_connection);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    refreshToken = reader.ToRefreshToken();
                }
                _connection.Close();
                return refreshToken;
            }
        }
        public void SaveRefreshToken(RefreshToken token)
        {
            using (SqlCommand cmd = _connection.CreateCommand())
            {
                cmd.CommandText = "INSERT INTO [RefreshToken] ([UserId], [Token], [ExpiredAt]) " +
                    "VALUES (@userId, @token, @expiresAt)";

                cmd.Parameters.AddWithValue("userId", token.UserId);
                cmd.Parameters.AddWithValue("token", token.Token);
                cmd.Parameters.AddWithValue("expiresAt", token.ExpiredAt);
               
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

        public void UseRefreshToken(int id)
        {
            using (SqlCommand cmd = _connection.CreateCommand())
            {
                cmd.CommandText = "UPDATE [RefreshToken] " +
                    "SET [Used] = 1 " +
                    "WHERE [Id] = @id";
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
