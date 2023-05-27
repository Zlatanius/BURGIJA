using System;

namespace Burgija.Models
{
    public class Administrator : User
    {
        #region Constructors

        public Administrator(int id, string username, string email, string password)
        { 
            Id=id;
            Username = username;
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