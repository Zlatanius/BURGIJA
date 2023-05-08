using System;

namespace Burgija.Models
{
    public class Store
    {
        #region Attributes

        private int id;
        private Location storeLocation;

        #endregion

        #region Properties

        public int Id { get =>  id; set => id = value; } 
        public Location StoreLocation { get => storeLocation; set => storeLocation = value; }

        #endregion

        #region Constructor

        public Store(int id, Location storeLocation) 
        { 
            Id = id;
            StoreLocation = storeLocation;
        }

        #endregion

        #region Methods

        //TODO  

        #endregion
    }
}