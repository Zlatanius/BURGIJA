using System;
using System.Collections.Generic;

namespace Burgija.Models
{
    public class RegisteredUser : User
    { 
        #region Constructors

        public RegisteredUser(int id, string username, string email, string password)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "Id mora biti ve�i od 0.");
            }

            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentException("Username ne mo�e biti null ili prazno.", nameof(username));
            }

            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentException("Email ne mo�e biti null ili prazno.", nameof(email));
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Password ne mo�e biti null ili prazno.", nameof(password));
            }
            Id = id;
            Username = username;
            Email = email;
            Password = password;
        }

        public RegisteredUser() { }

        #endregion

        #region Methods

        //TODO

        #endregion
    }
}