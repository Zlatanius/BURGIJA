using System;

namespace Burgija.Models
{
    public class Administrator : User
    {
        #region Constructors

        public Administrator(int id, string username, string name, string email, string password)
        { 
            Id=id;
            Username = username;
            Name=name;
            Email=email;
            Password=password;
        }
        
        public Administrator() { }
        #endregion

        #region Methods

        //TODO

        #endregion
    }

}