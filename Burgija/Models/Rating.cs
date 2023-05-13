using System;

namespace Burgija.Models
{
    public class Rating
    {
        #region Properties

        public int Id { get; set; }
        public double RatingValue { get; set; }

        #endregion

        #region Constructors

        public Rating(int id, double ratingValue)
        {
            Id = id;
            RatingValue = ratingValue;
        }

        public Rating() { }

        #endregion

        #region Methods

        //TODO

        #endregion
    }
}