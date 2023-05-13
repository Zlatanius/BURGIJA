using System;
using System.Collections.Generic;

namespace Burgija.Models
{
    public class RegisteredUser : User
    {
        #region Properties

        public List<Rent> RentHistory { get; set; }

        #endregion

        #region Constructors

        public RegisteredUser(int id, string username, string name, string email, string password, List<Rent> rentHistory)
        {
            Id = id;
            Username = username;
            Name = name;
            Email = email;
            Password = password;
            RentHistory = rentHistory;
        }

        public RegisteredUser() { }

        #endregion

        #region Methods

        //TODO

        #endregion
    }
}