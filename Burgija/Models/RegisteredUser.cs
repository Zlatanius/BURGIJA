using System;
using System.Collections.Generic;

namespace Burgija.Models
{
    public class RegisteredUser : User
    { 
        #region Constructors

        public RegisteredUser(int id, string username, string email, string password)
        {
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