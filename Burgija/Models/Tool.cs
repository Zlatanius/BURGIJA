using System;

namespace Burgija.Models
{
    public class Tool
    {
        #region Attributes

        private int id;
        private ToolType toolType;
        private List<Rent> rentList;
        private Store store;
        private double price;

        #endregion

        #region Properties

        public int Id { get => id; set => id = value; }
        public ToolType ToolType { get => toolType; set => toolType = value; }
        public List<Rent> ListRent { get => rentList; set => rentList = value; }
        public Store Store { get => store; set => store = value; } 
        public double Price { get => price; set => price = value; }

        #endregion

        #region Constructor

        public Store(int id, ToolType toolType, List<Rent> rentList, Store store, double price)
        {
            Id= id;
            ToolType = toolType;
            RentList = rentList;
            Store = store;
            Price = price;
        }

        #endregion

        #region Methods

        //TODO

        #endregion
    }
}