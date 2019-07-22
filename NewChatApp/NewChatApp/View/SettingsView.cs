using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewChatApp.View
{
    internal class SettingsView
    {
        public void SettingsViewer()
        {
            Console.WriteLine();
            Console.WriteLine("You are in Settins View! ./help");
            string[] line;
            do
            {
                Console.WriteLine("Enter some comand to do operation or ./back to leave");
                line = Console.ReadLine().Split().ToArray();
                if (line[0] == "/help")
                {
                    PrintAllComands();
                }
                else if (line[0] == "/Changepassword")
                {
                    string OldPassword = line[1];
                    string NewPassword = line[2];
                    //todo change password
                }
            } while (line[0] == "/back" || line[0] == "/BACK");
            return;
        }

        private void PrintAllComands()
        {
            Console.WriteLine("./back");
            Console.WriteLine("./Changepassword <oldpassword> <newpassword>");
        }
    }
}
