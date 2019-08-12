using System;
using System.Collections.Generic;
using System.Text;
using chatapplication;
using MySql.Data.MySqlClient;

namespace MyChatApplicationAzureServiceBus.Constructor
{
    public class LoginDataBaseInput
    {
        private MySqlConnection connection;
        private Login logindetails;
        private string server;
        private string database;
        private string uid;
        private string password;
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
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            connection = new MySqlConnection(connectionString);
        }
        //open connection to database
        private bool OpenConnection()
        {
            try
            {
                connection.OpenAsync();
                return true;
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        Console.WriteLine("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        Console.WriteLine("Invalid username/password, please try again");
                        break;
                }
                return false;
            }
        }

        //Close connection
        private bool CloseConnection()
        {
            try
            {
                connection.CloseAsync();
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public bool LoginUser()
        {
            string query = $"SELECT UserName,Password,Email from Participants where UserName = '{logindetails.Username}' and Password = '{logindetails.Password}'";
            if (OpenConnection())
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    dataReader.Close();
                    CloseConnection();
                    return true;
                }

                //close Data Reader
                dataReader.Close();
                CloseConnection();
                return false;
            }
            return false;
        }

    }
}
