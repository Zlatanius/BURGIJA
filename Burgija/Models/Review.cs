using System;

namespace Burgija.Models
{
    public class Review
    {
        #region Attributes

        private int id;
        private RegisteredUser user;
        private Tool tool;
        private Rent rent;
        private long timestamp;
        private string text;
        private Rating rating;

        #endregion

        #region Properties

        public int Id { get => id; set => id = value; } 
        public RegisteredUser User { get => user; set => user = value; }
        public Tool Tool { get => tool; set => tool = value; }
        public Rent Rent { get => rent; set => rent = value; }
        public long Timestamp { get => timestamp; set => timestamp = value; }
        public string Text { get => text; set => text = value; }
        public Rating Rating { get => rating; set => rating = value; }

        #endregion

        #region Constructor

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

        #endregion

        #region Methods

        //TODO

        #endregion
    }
}