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
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "Id mora biti veæi od 0.");
            }

            if (rent == null)
            {
                throw new ArgumentNullException(nameof(rent), "Rent ne može biti null.");
            }

            if (courier == null)
            {
                throw new ArgumentNullException(nameof(courier), "Kurir ne može biti null.");
            }

            if (string.IsNullOrEmpty(address))
            {
                throw new ArgumentException("Addresa ne može biti null ili prazna.", nameof(address));
            }
            if (string.IsNullOrEmpty(userPhoneNumber))
            {
                throw new ArgumentException("Broj telefona ne može biti null ili prazna.", nameof(userPhoneNumber));
            }

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