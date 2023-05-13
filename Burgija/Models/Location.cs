using System;

namespace Burgija.Models
{
    public class Location
    { 
        #region Properties

        public int Id { get; set; }
        public Tuple<double, double> Coordinates { get; set; }
        public string Address { get; set; }

        #endregion

        #region Constructors

        public Location(int id, Tuple<double, double> coordinates, string address)
        {
            Id = id;
            Address = address;
            Coordinates = coordinates;
        }

        public Location() { }

        #endregion

        #region Methods

            //TODO

        #endregion
    }
}