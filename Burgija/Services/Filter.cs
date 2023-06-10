using Burgija.Services;
using System.Collections.Generic;
using Burgija.Models;

namespace Burgija.Services
{
    public class Filter
    {
        private int? minPrice = null;
        private int? maxPrice = null;
        private IEnumerable<string>? manufacturers = null;
        public IEnumerable<string>? categories = null;
        private ISortStrategy? sortStrategy = null;
        //privatni konstruktor da zabranimo kreiranje filtera osim posredstvom buildera
        private Filter() { }

        public Filter(FilterBuilder builder)
        {
            minPrice = builder.minPrice;
            maxPrice = builder.maxPrice;
            
            sortStrategy = builder.sortStrategy;
        }

        public List<ToolType> getFilteredToolTypes(List<ToolType> tooltypes)
        {
            for (int i = 0; i < tooltypes.Count; i++)
            {
                var tooltype = tooltypes[i];

                if (minPrice != null && tooltype.Price < minPrice)
                {
                    tooltypes.Remove(tooltype);
                    i--;
                    continue;
                }
                if (maxPrice != null && tooltype.Price > maxPrice)
                {
                    tooltypes.Remove(tooltype);
                    i--;
                    continue;
                }
                
            }
            if (sortStrategy != null)
            {
                tooltypes = sortStrategy.sortToolTypes(tooltypes);
            }
            return tooltypes;
        }





    }
}