using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Burgija.Models
{
    public class Location
    {
        #region Properties

        [Key]
        public int Id { get; set; }
        public double XCoordinate { get; set; }
        public double YCoordinate { get; set; }
        public string Address { get; set; }

        #endregion

        #region Constructors

        public Location(int id, double xCoordinate, double yCoordinate, string address)
        {
            Id = id;
            XCoordinate = xCoordinate;
            YCoordinate = yCoordinate;
            Address = address;
        }

        public Location() { }

        #endregion

        #region Methods

        //TODO

        #endregion
    }
}