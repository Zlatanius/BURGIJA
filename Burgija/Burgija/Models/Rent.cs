using System;

namespace Burgija.Models
{
    public class Rent
    {
        #region Properties

        public int Id { get; set; }
        public RegisteredUser User { get; set; }
        public Tool Tool { get; set; }
        public DateTime StartOfRent { get; set; }
        public DateTime EndOfRent { get; set; }
        public Discount Discount { get; set; }

        #endregion

        #region Constructors

        public Rent(int id, RegisteredUser user, Tool tool, DateTime startOfRent, DateTime endOfRent, Discount discount)
        {
            Id = id;
            User = user;
            Tool = tool;
            StartOfRent = startOfRent;
            EndOfRent = endOfRent;
            Discount = discount;
        }

        public Rent() { }

        #endregion

        #region Methods

        //TODO

        #endregion
    }
}