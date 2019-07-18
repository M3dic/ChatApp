using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewChatApp.View
{
    internal class WellcomeView
    {
        public void Wellcome()
        {
            Console.WriteLine("Should pick a comand or ./help");
            string[] line;
            do
            {
                Console.WriteLine("Enter some comand to do operation or ./end to leave: ");
                line = Console.ReadLine().Split().ToArray();
                if (line[0] == "/help")
                {
                    PrintAllComands();
                }
                else if (line[0] == "/Login")
                {
                    LoginView lview = new LoginView();
                    lview.LoginViewer();
                }
                else if (line[0] == "/Register")
                {
                    RegisterView rview = new RegisterView();
                    rview.RegisterViewer();
                }
            } while (line[0] == "/end" || line[0] == "/END");
            return;
        }

        private void PrintAllComands()
        {
            Console.WriteLine("./Login");
            Console.WriteLine("./Register");
        }
    }
}
