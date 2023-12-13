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

        #endregion

        #region Constructors

        public Tool(int id, ToolType toolType, Store store)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "Id mora biti veæi 0.");
            }

            if (toolType == null)
            {
                throw new ArgumentNullException(nameof(toolType), "Tool type ne može biti null.");
            }

            if (store == null)
            {
                throw new ArgumentNullException(nameof(store), "Store ne može biti null.");
            }
            Id = id;
            ToolType = toolType;
            Store = store;
        }

        public Tool() { }

        #endregion

        #region Methods

        //TODO

        #endregion
    }
}