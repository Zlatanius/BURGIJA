using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Burgija.Models
{
    public class RentTool
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Rent")]
        public int RentId { get; set; }

        public Rent Rent { get; set; }

        [ForeignKey("Tool")]
        public int ToolId { get; set; }

        public Tool Tool { get; set; }
    }
}
