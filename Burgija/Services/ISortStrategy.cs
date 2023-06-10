using Burgija.Models;
using System.Collections.Generic;

namespace Burgija.Services
{
    public interface ISortStrategy
    {
        public List<ToolType> sortToolTypes(List<ToolType> tooltypes);
    }
}
