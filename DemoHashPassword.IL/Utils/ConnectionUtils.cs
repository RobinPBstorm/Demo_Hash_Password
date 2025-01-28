using System.Data;
using Microsoft.Data.SqlClient;

namespace DemoHashPassword.IL.Utils
{
    public static class ConnectionUtils
    {

        public static void OpenConnection(SqlConnection connection )
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
            connection.Open();
        }
    }
}
