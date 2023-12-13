using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Burgija.Models
{
    public class Review
    {
        #region Properties

        [Key]
        public int Id { get; set; }
        public IdentityUser<int> User { get; set; }

        [ForeignKey("AspNetUsers")]
        public int UserId { get; set; } 
        public Tool Tool { get; set; }

        [ForeignKey("Tool")]
        public int ToolId { get; set; }
        public Rent Rent { get; set; }

        [ForeignKey("Rent")]
        public int RentId { get; set; }
        public long Timestamp { get; set; }
        public string Text { get; set; }
        
        public double Rating { get; set; }

        #endregion

        #region Constructors

        public Review(int id, IdentityUser<int> user, Tool tool, Rent rent, long timestamp, string text, double rating)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "Id mora biti veæi od 0.");
            }

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User ne može biti null.");
            }

            if (tool == null)
            {
                throw new ArgumentNullException(nameof(tool), "Tool ne može biti null.");
            }
            if (rent == null)
            {
                throw new ArgumentNullException(nameof(rent), "Rent ne može biti null.");
            }

            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentException("Text ne može biti null ili prazan.", nameof(text));
            }

            if (rating < 0 || rating > 5)
            {
                throw new ArgumentOutOfRangeException(nameof(rating), "Rating mora biti izmeðu 0 i 5.");
            }


            Id = id;
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