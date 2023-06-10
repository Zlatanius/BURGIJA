using Burgija.Services;
using System.Collections.Generic;
using Burgija.Models;
using System.Linq;

namespace Burgija.Services
{
    public class LowestPriceStrategy : ISortStrategy
    {
        public List<ToolType> sortToolTypes(List<ToolType> tooltypes)
        {
            return tooltypes.OrderByDescending(t => t.Price).ToList();
        }
    }
}