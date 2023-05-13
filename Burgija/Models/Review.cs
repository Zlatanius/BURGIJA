using System;

namespace Burgija.Models
{
    public class Review
    {
        #region Properties

        public int Id { get; set; }
        public RegisteredUser User { get; set; }
        public Tool Tool { get; set; }
        public Rent Rent { get; set; }
        public long Timestamp { get; set; }
        public string Text { get; set; }
        public Rating Rating { get; set; }

        #endregion

        #region Constructors

        public Review(int id, RegisteredUser user, Tool tool, Rent rent, long timestamp, string text, Rating rating)
        {
            Id=id;
            User=user;
            Tool=tool;
            Rent=rent;
            Timestamp=timestamp;
            Text=text;
            Rating=rating;
        }

        public Review() { }

        #endregion

        #region Methods

        //TODO

        #endregion
    }
}