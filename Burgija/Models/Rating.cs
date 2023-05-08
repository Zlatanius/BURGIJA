using System;

namespace Burgija.Models
{
    public class Rating
    {
        #region Attributes

        private int id;
        private double rating;

        #endregion

        #region Properties

        public int Id { get => id; set => id = value; }
        public double Rating { get => rating; set => rating = value; }

        #endregion

        #region Constructor

        public Rating(int id, double rating)
        {
            Id = id;
            Rating = rating;
        }

        #endregion

        #region Methods

        //TODO

        #endregion
    }
}