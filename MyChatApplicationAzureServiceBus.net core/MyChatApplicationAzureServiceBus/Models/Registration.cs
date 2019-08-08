using System;

namespace chatapplication
{
    public class Registration
    {
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
        }
        public string Name { get; private set; }
        public string Password { get; private set; }
        public Guid SecretNumber { get; private set; }
        public string Email { get; private set; }
    } 
}
