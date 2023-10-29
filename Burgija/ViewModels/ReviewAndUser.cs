using System;
using Microsoft.AspNetCore.Identity;

namespace Burgija.ViewModels
{
    public class ReviewAndUser
    {
        public double Rating { get; set; }
        public string Text { get; set; }
        public string UserName { get; set; }

        public ReviewAndUser(double rating, string text, string userName)
        {
            Rating = rating;
            Text = text;
            UserName = userName;
        }
    }
}