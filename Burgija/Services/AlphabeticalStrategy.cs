using Burgija.Services;
using System.Collections.Generic;
using Burgija.Models;
using System.Linq;

namespace Burgija.Services
{
    public class AlphabeticalStrategy : ISortStrategy
    {
        public List<ToolType> sortToolTypes(List<ToolType> tooltypes)
        {
            return tooltypes.OrderBy(t => (t.Name)).ToList();
        }
    }
}