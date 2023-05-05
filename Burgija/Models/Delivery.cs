using System;

namespace Burgija.Models
{
    public class Delivery
    {
        #region Attributes

        private int id;
        private Rent rent;
        private Courier courier;
        private string address;
        private string userPhoneNumber;

        #endregion

        #region Properties

        public int Id { get => id; set => id = value; }
        public Rent Rent { get => rent; set => rent = value; }
        public Courier Courier { get => courier; set => courier = value; }
        public string Address { get => address; set => address = value; }
        public string UserPhoneNumber { get => userPhoneNumber; set => userPhoneNumber = value; }

        #endregion

        #region Constructor

        public Delivery(int id, Rent rent, Courier courier, string address, string userPhoneNumber)
        {
            Id = id;
            Rent = rent;
            Courier = courier;
            Address = address;
            UserPhoneNumber = userPhoneNumber;
        }

        #endregion

        #region Methods

        //TODO

        #endregion
    }
}