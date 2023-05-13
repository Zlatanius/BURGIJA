using System;

namespace Burgija.Models
{
    public abstract class User
    {
        #region Properties

        public int Id { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        #endregion

        #region Methods

        //TODO

        #endregion
    }
}