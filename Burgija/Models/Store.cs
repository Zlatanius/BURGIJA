using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Burgija.Models
{
    public class Store
    {
        #region Properties

        [Key]
        public int Id { get; set; }
        public Location StoreLocation { get; set; }

        [ForeignKey("Location")]
        public int LocationId { get; set; }
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