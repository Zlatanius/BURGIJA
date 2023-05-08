using System;

namespace Burgija.Models
{
    public class RegisteredUser : User
    {
        #region Attributes

        private List<Rent> rentHistory;

        #endregion

        #region Properties

        public List<Rent> RentHistory { get=>rentHistory; set => rentHistory = value; }

        #endregion

        #region Constructor

        public RegisteredUser(int id, string username, string name, string email, string password, List<Rent> rentHistory)
        {
            Id = id;
            Username = username;
            Name = name;
            Email = email;
            Password = password;
            RentHistory = rentHistory;
        }

        #endregion

        #region Methods

        //TODO

        #endregion
    }
}