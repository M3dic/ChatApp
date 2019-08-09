using chatapplication;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyChatApplicationAzureServiceBus.Constructor
{
    public class RegistrationDataBaseInput
    {
        private Registration registrationinforamtion;
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;
        public RegistrationDataBaseInput(Registration registration)
        {
            registrationinforamtion = registration;
            Initialize();
        }
        private void Initialize()
        {
            server = "chatapplication0.mysql.database.azure.com";
            database = "ChatApp";
            uid = "Ivo@chatapplication0";
            password = "Levski.1914";
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
                connection.Open();
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
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public bool RegisterNewPartisipant()
        {
            string query = $"INSERT INTO Participants" +
                $" (SecretNumber,UserName,Password,Email) " +
                $"VALUES('{registrationinforamtion.SecretNumber}', '{registrationinforamtion.Name}', '{registrationinforamtion.Password}', '{registrationinforamtion.Email}')";
            if (OpenConnection())
            {
                if (CheckForUserName())
                {
                    //create command and assign the query and connection from the constructor
                    MySqlCommand cmd = new MySqlCommand(query, connection);

                    //Execute command
                    cmd.ExecuteNonQuery();

                    //close connection
                    this.CloseConnection();
                    return true;
                }
            }
            return false;
        }

        private bool CheckForUserName()
        {
            string query = $"select UserName from Participants";
            List<string> list = new List<string>();
            //Create Command
            MySqlCommand cmd = new MySqlCommand(query, connection);
            //Create a data reader and Execute the command
            MySqlDataReader dataReader = cmd.ExecuteReader();

            //Read the data and store them in the list
            while (dataReader.Read())
            {
                list.Add(dataReader["UserName"].ToString().ToLowerInvariant());
            }

            //close Data Reader
            dataReader.Close();

            if (list.Contains(registrationinforamtion.Name))
            {
                Console.WriteLine("UserName already exists");
                return false;
            }
            return true;
        }
    }
}
