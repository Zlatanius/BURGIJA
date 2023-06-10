using System.Collections.Generic;

namespace Burgija.Services
{
    public interface IFilterBuilder
    {
        public Filter Build();
        public void AddMinPrice(int min);
        public void AddMaxPrice(int max);
        
        public void AddSortStrategy(SortType sortType);
    }
}
