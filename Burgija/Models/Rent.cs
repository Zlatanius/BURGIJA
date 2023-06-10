using Microsoft.AspNetCore.Identity;
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
        public IdentityUser<int> User { get; set; }

        [ForeignKey("AspNetUsers")]
        public int UserId { get; set; }
        public Tool Tool { get; set; }

        [ForeignKey("Tool")]
        public int ToolId { get; set; }
        public DateTime StartOfRent { get; set; }
        public DateTime EndOfRent { get; set; }
        public Discount Discount { get; set; }

        [ForeignKey("Discount")]
        public int? DiscountId { get; set; }
        public double RentPrice { get; set; }

        #endregion

        #region Constructors

        public Rent(int id, IdentityUser<int> user, Tool tool, DateTime startOfRent, DateTime endOfRent, Discount discount)
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