using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Burgija.Models
{
    public class Tool
    {
        #region Properties
        [Key] 
        public int Id { get; set; }
        public ToolType ToolType { get; set; }

        [ForeignKey("ToolType")]
        public int ToolTypeId { get; set; }
        public Store Store { get; set; }

        [ForeignKey("Store")]
        public int StoreId{ get; set; }
        public double Price { get; set; }

        #endregion

        #region Constructors

        public Tool(int id, ToolType toolType, Store store, double price)
        {
            Id= id;
            ToolType = toolType;
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