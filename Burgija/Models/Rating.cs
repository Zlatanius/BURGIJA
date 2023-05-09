using System;

namespace Burgija.Models
{
    public class Rating
    {
        #region Attributes

        private int id;
        private double ratingValue;

        #endregion

        #region Properties

        public int Id { get => id; set => id = value; }
        public double RatingValue { get => ratingValue; set => ratingValue = value; }

        #endregion

        #region Constructor

        public Rating(int id, double ratingValue)
        {
            Id = id;
            RatingValue = ratingValue;
        }

        #endregion

        #region Methods

        //TODO

        #endregion
    }
}