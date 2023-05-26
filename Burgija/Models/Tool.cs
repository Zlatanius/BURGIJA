using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Burgija.Models
{
    public class Tool
    {
        #region Properties
        [DisplayName("Serial code")]
        [Key] 
        public int Id { get; set; }
        [DisplayName("Tool Type")]
        public ToolType ToolType { get; set; }

        [DisplayName("Tool Type")]
        [ForeignKey("ToolType")]
        public int ToolTypeId { get; set; }
        public Store Store { get; set; }

        [DisplayName("Store")]
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