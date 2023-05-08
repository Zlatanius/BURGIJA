using System;

namespace Burgija.Models
{
    public abstract class User
    {
        #region Attributes

        private int id;
        private string username;
        private string name;
        private string email;
        private string password;

        #endregion

        #region Properties

        public int Id { get => id; set => id = value; }
        public string Username { get => username; set => username = value; }
        public string Name { get => name; set => name = value; }
        public string Email { get => email; set => email = value; }
        public string Password { get => password; set => password = value; }

        #endregion

        #region Methods

        //TODO

        #endregion
    }
}