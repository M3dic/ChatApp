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
        private string server;
        private string database;
        private string uid;
        private string password;
        private string connectionString;

        public RegistrationDataBaseInput(Registration registration)
        {
            registrationinforamtion = registration ?? throw new ArgumentNullException(nameof(registration));
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

        public bool RegisterNewPartisipant()
        {
            string query = $"INSERT INTO Participants" +
                $" (SecretNumber,UserName,Password,Email) " +
                $"VALUES('{registrationinforamtion.SecretNumber}', '{registrationinforamtion.Name}', '{registrationinforamtion.Password}', '{registrationinforamtion.Email}')";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.OpenAsync();

                if (CheckForUserName())
                {
                    //create command and assign the query and connection from the constructor
                    MySqlCommand cmd = new MySqlCommand(query, connection);

                    //Execute command
                    cmd.ExecuteNonQuery();

                    //close connection
                    connection.CloseAsync();
                    return true;
                }
                connection.CloseAsync();
            }
            return false;
        }

        private bool CheckForUserName()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
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
                    connection.CloseAsync();
                    return false;
                }
                connection.CloseAsync();
                return true;
            }
        }
    }
}
