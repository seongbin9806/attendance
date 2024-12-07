using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace 출결관리프로그램
{
    public class DatabaseHelper
    {
        private string connectionString;

        public DatabaseHelper(string server, string database, string uid, string password)
        {
            connectionString = $"Server={server};Database={database};Uid={uid};Pwd={password};";
        }

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }

        public void OpenConnection(MySqlConnection connection)
        {
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
        }

        public void CloseConnection(MySqlConnection connection)
        {
            if (connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }
        }

        public MySqlDataReader ExecuteQuery(string query)
        {
            MySqlConnection connection = GetConnection();
            MySqlCommand command = new MySqlCommand(query, connection);

            OpenConnection(connection);
            MySqlDataReader reader = command.ExecuteReader();
            CloseConnection(connection);

            return reader;
        }

        public void ExecuteNonQuery(string query)
        {
            MySqlConnection connection = GetConnection();
            MySqlCommand command = new MySqlCommand(query, connection);

            OpenConnection(connection);
            command.ExecuteNonQuery();
            CloseConnection(connection);
        }

        public bool TestConnection()
        {
            try
            {
                using (MySqlConnection connection = GetConnection())
                {
                    OpenConnection(connection);
                    CloseConnection(connection);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
