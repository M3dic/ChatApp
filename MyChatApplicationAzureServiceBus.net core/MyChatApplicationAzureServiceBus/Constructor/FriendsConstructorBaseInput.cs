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
            if (string.IsNullOrWhiteSpace(usernaem))
                throw new ArgumentException("message", nameof(usernaem));

            if (string.IsNullOrWhiteSpace(friendname))
                throw new ArgumentException("message", nameof(friendname));

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
        internal void GetInvitaions(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("message", nameof(username));

            string query = $"Select * from Friends where FriendsUsername = '{username}' and Accepted = 'N'";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.OpenAsync();
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);

                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    Console.WriteLine("Username: "+ dataReader["UserName"].ToString().ToLowerInvariant()+" has invited you.");
                    Console.Write("Do you want to accept him (Y/N): ");
                    string answer = Console.ReadLine().ToUpperInvariant();
                    if (answer == "Y")
                    {
                        AcceptFriendInvitation(username,dataReader["UserName"].ToString().ToLowerInvariant());
                    }
                    else if(answer == "N")
                    {
                        RemoveInvitation(dataReader["UserName"].ToString().ToLowerInvariant(), username);
                    }
                    else
                    {
                        Console.WriteLine("Not answered!");
                    }
                }

                //close Data Reader
                dataReader.Close();

                //close connection
                connection.CloseAsync();
            }
        }

        private void RemoveInvitation(string friendusername, string username)
        {
            if (string.IsNullOrWhiteSpace(friendusername))
                throw new ArgumentException("message", nameof(friendusername));

            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("message", nameof(username));

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.OpenAsync();
                string query = $"DELETE FROM Friends WHERE UserName='{friendusername}' and FriendsUsername = '{username}'";
                MySqlCommand cmd = new MySqlCommand(query,connection);
                cmd.ExecuteNonQuery();
                connection.CloseAsync();
            }
         
        }

        private void AcceptFriendInvitation(string username, string friendusername)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("message", nameof(username));

            if (string.IsNullOrWhiteSpace(friendusername))
                throw new ArgumentException("message", nameof(friendusername));

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.OpenAsync();
                string query = $"Update Friends Set Accepted = 'Y' where UserName = '{friendusername}' and FriendsUsername = '{username}'";
                string query1 = $"INSERT INTO Friends" +
                   $" (UserName,FriendsUsername,Accepted) " +
                   $"VALUES('{username}', '{friendusername}', 'Y')";
                MySqlCommand cmd = new MySqlCommand(query,connection);
                cmd.ExecuteNonQuery();
                cmd.CommandText = query1;
                cmd.ExecuteNonQuery();
                connection.CloseAsync();
                Console.WriteLine("Successfully added new friend");
            }
        }

        public HashSet<string> GetFriendsNames(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("message", nameof(username));

            string query = $"SELECT FriendsUsername FROM ChatApp.Friends where UserName = '{username}' and Accepted = 'Y'";
            HashSet<string> names = new HashSet<string>();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.OpenAsync();
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    names.Add(dataReader["FriendsUsername"].ToString());
                }

                //close Data Reader
                dataReader.Close();

                //close connection
                connection.CloseAsync();
            }
            return names;
        }
    }
}
