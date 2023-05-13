using System;

namespace Burgija.Models
{
    public class Delivery
    {
        #region Properties

        public int Id { get; set; }
        public Rent Rent { get; set; }
        public Courier Courier { get; set; }
        public string Address { get; set; }
        public string UserPhoneNumber { get; set; }

        #endregion

        #region Constructors

        public Delivery(int id, Rent rent, Courier courier, string address, string userPhoneNumber)
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

        //TODO

        #endregion
    }
}