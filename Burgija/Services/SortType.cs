using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Burgija.Services
{
    public enum SortType
    {
        [Display(Name = "Lowest price first")]
        LowestFirst,
        [Display(Name = "Highest price first")]
        HighestFirst,
        [Display(Name = "Alphabetical")]
        Alphabetical
    }
}
