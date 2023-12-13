using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Burgija.Models
{
    public class Rent : IComparable<Rent>
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

        
        public Rent() { }

        public Rent(int id, IdentityUser<int> user, int userId, Tool tool, int toolId, DateTime startOfRent, DateTime endOfRent, Discount discount, int? discountId, double rentPrice)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "Id mora biti veæi od 0.");
            }

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User ne može biti null.");
            }

            if (tool == null)
            {
                throw new ArgumentNullException(nameof(tool), "Tool ne može biti null.");
            }

            if (startOfRent >= endOfRent)
            {
                throw new ArgumentException("Datum poèetka renta mora biti prije datuma kraja.");
            }

            if (rentPrice < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(rentPrice), "Cijena renta ne može biti negativna.");
            }
            Id = id;
            User = user;
            UserId = userId;
            Tool = tool;
            ToolId = toolId;
            StartOfRent = startOfRent;
            EndOfRent = endOfRent;
            Discount = discount;
            DiscountId = discountId;
            RentPrice = rentPrice;
        }

        #endregion

        #region Methods

        int IComparable<Rent>.CompareTo(Rent other)
        {
            return RentPrice.CompareTo(other.RentPrice);
        }

        #endregion
    }
}