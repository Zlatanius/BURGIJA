using Burgija.Services;
using System.Collections.Generic;
using Burgija.Models;
using System.Linq;

namespace TechHaven.Services
{
    public class FilterMediator : IFilterMediator
    {
        private IFilterBuilder _filterBuilder;
        private List<ToolType> tooltypes;

        public FilterMediator(FilterBuilder filterBuilder)
        {
            _filterBuilder = filterBuilder;
        }

        public IEnumerable<ToolType> GetFilteredToolTypes()
        {
            return _filterBuilder.Build().getFilteredToolTypes(tooltypes);
        }

        public void SetConditions(IEnumerable<ToolType> tooltypes, int minPrice, int maxPrice, SortType sortType)
        {
            if (minPrice != 0)
            {
                _filterBuilder.AddMinPrice(minPrice);
            }
            if (maxPrice != 0)
            {
                _filterBuilder.AddMaxPrice(maxPrice);
            }
            
            _filterBuilder.AddSortStrategy(sortType);
            this.tooltypes = tooltypes.ToList();
        }
    }
}