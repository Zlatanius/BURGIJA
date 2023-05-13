using System;
using System.Collections.Generic;

namespace Burgija.Models
{
    public class Tool
    {
        #region Properties

        public int Id { get; set; }
        public ToolType ToolType { get; set; }
        public List<Rent> RentList { get; set; }
        public Store Store { get; set; }
        public double Price { get; set; }

        #endregion

        #region Constructors

        public Tool(int id, ToolType toolType, List<Rent> rentList, Store store, double price)
        {
            Id= id;
            ToolType = toolType;
            RentList = rentList;
            Store = store;
            Price = price;
        }

        public Tool() { }

        #endregion

        #region Methods

        //TODO

        #endregion
    }
}