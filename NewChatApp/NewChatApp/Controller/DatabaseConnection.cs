using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace NewChatApp.Controller
{
    public static class DatabaseConnection
    {
        static MySqlConnection databaseConnection = null;
        public static MySqlConnection getDBConnection()
        {
            if (databaseConnection == null)
            {
                string connectionString = ConfigurationManager.ConnectionStrings["myDatabaseConnection"].ConnectionString;
                databaseConnection = new MySqlConnection(connectionString);
            }
            return databaseConnection;
        }
    }
}
