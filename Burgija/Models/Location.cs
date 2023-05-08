using System;

namespace Burgija.Models
{
    public class Location
    {
        #region Attributes

        private int id;
        private Tuple<double, double> coordinates;
        private string address;

        #endregion

        #region Properties

        public int Id { get => id; set => id = value; }
        public Tuple<double, double> Coordinates { get => coordinates; set => coordinates = value; }
        public string Address { get => address; set => address = value; }

        #endregion

        #region Constructor

        public Location(int id, Tuple<double, double> coordinates, string address)
        {
            Id = id;
            Address = address;
            Coordinates = coordinates;
        }

        #endregion

        #region Methods

            //TODO

        #endregion
    }
}