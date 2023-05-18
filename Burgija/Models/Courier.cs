using System;
using System.Collections.Generic;

namespace Burgija.Models
{
    public class Courier : User
    { 
        #region Constructors

        public Courier(int id, string username, string name, string email, string password)
        {
            Id = id;
            Username = username;
            Name = name;
            Email = email;
            Password = password;
        }

        public Courier() { }

        #endregion

        #region Methods

        //TODO

        #endregion
    }
}