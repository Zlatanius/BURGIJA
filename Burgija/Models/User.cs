using System;
using System.ComponentModel.DataAnnotations;

namespace Burgija.Models
{
    public abstract class User
    {
        #region Properties

        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

        #endregion

        #region Methods

        //TODO

        #endregion
    }
}