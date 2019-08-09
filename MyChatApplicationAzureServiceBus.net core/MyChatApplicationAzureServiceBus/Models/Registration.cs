using ChatServiceBus;
using MyChatApplicationAzureServiceBus.Constructor;
using System;
using System.Net.Mail;
using System.Text;

namespace chatapplication
{
    public class Registration
    {

        public string Name { get; private set; }
        public string Password { get; private set; }
        public Guid SecretNumber { get; private set; }
        public string Email { get; private set; }

        public bool isRegistered = false;
        public Registration(string name, string password, string email)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("message", nameof(name));

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("message", nameof(password));

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("message", nameof(email));

            Name = name;
            Password = password;
            Email = email;
            SecretNumber = Guid.NewGuid();
            RegistrationDataBaseInput registration = new RegistrationDataBaseInput(this);
            if (registration.RegisterNewPartisipant())
            {
                SendMessageToEmail();
                Helper.CreateSubscription(Name);
                Console.WriteLine("Successfully registered");
                this.isRegistered = true;
            }
            else
            {
                this.isRegistered = false;
            }
        }

        private void SendMessageToEmail()//TODO ERROR
        {
            try
            {
                SmtpClient client = new SmtpClient();
                client.Port = 587;
                client.Host = "smtp.gmail.com";
                client.EnableSsl = true;
                client.Timeout = 10000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential("hospitalm3dic@gmail.com", "08857490200");

                MailMessage mm = new MailMessage("donotreply@domain.com", $"{this.Email}", "Successfully registered account", $"\nUser: {this.Name} \nPassword: {this.Password}");
                mm.BodyEncoding = UTF8Encoding.UTF8;
                mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                client.Send(mm);
                Console.WriteLine("~~Check your email for registration details~~");
            }
            catch (Exception ex)
            {

            }
        }

    } 
}
