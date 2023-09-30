using Burgija.Models;

namespace Burgija.ViewModels
{
    public class RentAndToolType
    {
        public Rent Rent { get; set; }
        public ToolType ToolType { get; set; }
        public RentAndToolType(Rent rent, ToolType toolType)
        {
            Rent = rent;
            ToolType = toolType;
        }
    }
}
