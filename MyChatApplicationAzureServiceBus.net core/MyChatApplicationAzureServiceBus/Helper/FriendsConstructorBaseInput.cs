using ChatServiceBus;
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
        private static string connectionString;

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
            connectionString = "SERVER=" + server + ";" + "DATABASE=" + database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
        }

        internal void InviteFriend(string usernaem, string friendname)
        {
            if (string.IsNullOrWhiteSpace(usernaem))
                throw new ArgumentNullException(nameof(usernaem));

            if (string.IsNullOrWhiteSpace(friendname))
                throw new ArgumentNullException(nameof(friendname));

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
                throw new ArgumentNullException(nameof(username));

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

                dataReader.Close();

                connection.CloseAsync();
            }
        }

        private void RemoveInvitation(string friendusername, string username)
        {
            if (string.IsNullOrWhiteSpace(friendusername))
                throw new ArgumentNullException(nameof(friendusername));

            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentNullException(nameof(username));

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
                throw new ArgumentNullException(nameof(username));

            if (string.IsNullOrWhiteSpace(friendusername))
                throw new ArgumentNullException(nameof(friendusername));

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.OpenAsync();
                string topicname = username + " " + friendusername;

                string query = $"Update Friends Set Accepted = 'Y' where UserName = '{friendusername}' and FriendsUsername = '{username}'";

                string query1 = $"INSERT INTO Friends" +
                   $" (UserName,FriendsUsername,Accepted) " +
                   $"VALUES('{username}', '{friendusername}', 'Y')";

                string query2 = $"Create table {topicname}(string SenderName varchar(50) not null, string Messege varchar(2000));";

                MySqlCommand cmd = new MySqlCommand(query, connection);

                cmd.ExecuteNonQuery();
                cmd.CommandText = query1;
                cmd.ExecuteNonQuery();
                cmd.CommandText = query2;
                cmd.ExecuteNonQuery();

                connection.CloseAsync();
                AzureServiceBusHelper.CreateTopic(topicname);
                AzureServiceBusHelper.CreateSubscription(topicname, new List<string>() { username, friendusername });
                Console.WriteLine("Successfully added new friend");
            }
        }

        static internal IEnumerable<string> GetSubscriptionsNames(string username)
        {
            string query = $"SELECT UserName FROM ChatApp.Participants where UserName != '{username}'";

            List<string> list = new List<string>();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.OpenAsync();
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);

                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    list.Add("  " + dataReader["UserName"].ToString().ToLowerInvariant());
                }

                //close Data Reader
                dataReader.Close();

                connection.CloseAsync();
            }
            return list;
        }

        public HashSet<string> GetFriendsNames(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentNullException(nameof(username));

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
