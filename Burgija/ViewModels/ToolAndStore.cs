using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Burgija.ViewModels
{
    [NotMapped]
    public class ToolAndStore
    {
        public string Address { get; set; }
        public int StoreId { get; set; }
        public int Quantity { get; set; }

        public ToolAndStore(string address, int storeId, int quantity)
        {
            Address = address;
            StoreId = storeId;
            Quantity = quantity;
        }
    }
}
