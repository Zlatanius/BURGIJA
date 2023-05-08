using System;

namespace Burgija.Models
{
    public class Administrator : User
    {
        #region Constructor

        public Administrator(int id, string username, string name, string email, string password)
        { 
            Id=id;
            Username = username;
            Name=name;
            Email=email;
            Password=password;
        }

        #endregion

        #region Methods

        //TODO

        #endregion
    }

}