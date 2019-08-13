using System;
using System.Collections.Generic;
using System.Text;
using chatapplication;
using MySql.Data.MySqlClient;

namespace MyChatApplicationAzureServiceBus.Constructor
{
    public class LoginDataBaseInput
    {
        private Login logindetails;
        private string server;
        private string database;
        private string uid;
        private string password;
        private string connectionString;

        public LoginDataBaseInput(Login login)
        {
            logindetails = login;
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
       
        public bool LoginUser()
        {
            string query = $"SELECT UserName,Password,Email from Participants where UserName = '{logindetails.Username}' and Password = '{logindetails.Password}'";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.OpenAsync();
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    dataReader.Close();
                    connection.CloseAsync();
                    return true;
                }

                //close Data Reader
                dataReader.Close();
                connection.CloseAsync();
                return false;
            }
        }

    }
}
