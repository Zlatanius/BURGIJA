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
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "Id mora biti veæi od 0.");
            }

            if (storeLocation == null)
            {
                throw new ArgumentNullException(nameof(storeLocation), "Lokacija trgovine ne može biti null.");
            }
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