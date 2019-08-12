using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyChatApplicationAzureServiceBus.Constructor
{
    public class FriendsConstructorBaseInput
    {
        private string server;
        private string database;
        private string uid;
        private string password;
        private string connectionString;

        public FriendsConstructorBaseInput()
        {
            Initialize();
        }

        private void Initialize()
        {
            server = "35.202.2.242";
            database = "ChatApp";
            uid = "Ivo";
            password = "123456789";
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
        }

        internal void InviteFriend(string usernaem, string friendname)
        {
            string query = $"INSERT INTO Friends" +
                $" (UserName,FriendsUsername) " +
                $"VALUES('{usernaem}', '{friendname}')";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.OpenAsync();
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                connection.CloseAsync();
            }
        }
    }
}
