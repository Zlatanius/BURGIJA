using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Burgija.Models
{
    [NotMapped]
    public class ToolAndStore
    {
        public string Address { get; set; }
        public int StoreId { get; set; }
        public int Quantity { get; set; }
    }
}
