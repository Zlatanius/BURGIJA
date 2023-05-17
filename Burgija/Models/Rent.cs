using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Burgija.Models
{
    public class Rent
    {
        #region Properties

        [Key]
        public int Id { get; set; }
        public RegisteredUser User { get; set; }

        [ForeignKey("RegisteredUser")]
        public int UserId { get; set; }
        public Tool Tool { get; set; }
        public DateTime StartOfRent { get; set; }
        public DateTime EndOfRent { get; set; }
        public Discount Discount { get; set; }

        [ForeignKey("Discount")]
        public int DiscountId { get; set; }

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