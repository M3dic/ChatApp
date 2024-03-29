﻿using ChatServiceBus;
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
                throw new ArgumentNullException(nameof(name));

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException(nameof(password));

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentNullException(nameof(email));

            Name = name;
            Password = password;
            Email = email;
            SecretNumber = Guid.NewGuid();
            RegistrationDataBaseInput registration = new RegistrationDataBaseInput(this);
            if (registration.RegisterNewPartisipant())
            {
                SendMessageToEmail();
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
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("ivoradev14@gmail.com");
                mail.To.Add($"{this.Email}");
                mail.Subject = "Test Mail";
                mail.Body = "This is for testing SMTP mail from GMAIL";

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("ivoradev14@gmail.com", "");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
                Console.WriteLine("~~Check your email for registration details~~");
            }
            catch (Exception)
            {

            }
        }

    } 
}
