using Burgija.Services;
using System.Collections.Generic;
using Burgija.Models;

namespace Burgija.Services
{
    public interface IFilterMediator
    {
        public IEnumerable<ToolType> GetFilteredToolTypes();
        public void SetConditions(IEnumerable<ToolType> tooltypes, int minPrice, int maxPrice, SortType sortType);
    }
}