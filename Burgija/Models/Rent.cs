using System;

namespace Burgija.Models
{
    public class Rent
    {
        #region Attributes

        private int id;
        private RegisteredUser user;
        private Tool tool;
        private DateTime startOfRent;
        private DateTime endOfRent;
        private Discount discount;

        #endregion

        #region Properties

        public int Id { get =>  id; set => id = value; }  
        public RegisteredUser user { get => user; set => user = value; }
        public Tool tool { get => tool; set => tool = value; }
        public DateTime StartOfRent { get => startOfRent; set => startOfRent = value; }
        public DateTime EndOfRent { get => endOfRent; set => endOfRent = value; }
        public Discount discount { get => Discount; set => discount = value; }

        #endregion

        #region Constructor

        public Rent(int id, RegisteredUser user, Tool tool, DateTime startOfRent, DateTime endOfRent, Discount discount) 
        {
            Id = id;
            User = user;
            Tool = tool;
            StartOfRent = startOfRent;
            EndOfRent = endOfRent;
            Discount = discount;
        }

        #endregion

        #region Methods

        //TODO

        #endregion
    }
