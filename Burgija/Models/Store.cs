using System;

namespace Burgija.Models
{
    public class Store
    {
        #region Properties

        public int Id { get; set; }
        public Location StoreLocation { get; set; }

        #endregion

        #region Constructors

        public Store(int id, Location storeLocation) 
        { 
            Id = id;
            StoreLocation = storeLocation;
        }

        public Store() { }

        #endregion

        #region Methods

        //TODO  

        #endregion
    }
}