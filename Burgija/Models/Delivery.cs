using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Burgija.Models
{
    public class Delivery : IComparable<Delivery>
    {
        #region Properties

        [Key]
        public int Id { get; set; }
        public Rent Rent { get; set; }

        [ForeignKey("Rent")]
        public int RentId { get; set; }
        public IdentityUser<int> Courier { get; set; }
        
        [ForeignKey("AspNetUsers")]
        public int? CourierId { get; set; }
        [Required]
        public string Address { get; set; }
        public string UserPhoneNumber { get; set; }

        #endregion

        #region Constructors

        public Delivery(int id, Rent rent, IdentityUser<int> courier, string address, string userPhoneNumber)
        {
            Id = id;
            Rent = rent;
            Courier = courier;
            Address = address;
            UserPhoneNumber = userPhoneNumber;
        }

        public Delivery() { }

        #endregion

        #region Methods

        public int CompareTo(Delivery other)
        {
            return Rent.StartOfRent.CompareTo(other.Rent.StartOfRent);
        }

        #endregion
    }
}