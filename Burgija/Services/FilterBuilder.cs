using Burgija.Services;
using System.Collections.Generic;

namespace Burgija.Services
{
    public class FilterBuilder : IFilterBuilder
    {
        public int? minPrice { get; private set; } = null;
        public int? maxPrice { get; private set; } = null;
        
        public ISortStrategy? sortStrategy { get; private set; } = null;


        public void AddMaxPrice(int max)
        {
            if (max > 0)
            {
                maxPrice = max;
            }
        }

        public void AddMinPrice(int min)
        {
            if (min > 0)
            {
                minPrice = min;
            }
        }

        public void AddSortStrategy(SortType sortType)
        {
            switch (sortType)
            {
                case SortType.HighestFirst:
                    sortStrategy = new HighestPriceStrategy();
                    break;
                case SortType.LowestFirst:
                    sortStrategy = new LowestPriceStrategy();
                    break;
                case SortType.Alphabetical:
                    sortStrategy = new AlphabeticalStrategy();
                    break;
            }
        }

        

        

        public Filter Build()
        {
            return new Filter(this);
        }
    }
}