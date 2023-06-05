using System;
using System.ComponentModel.DataAnnotations;

namespace Burgija.Models
{
    public class ToolType
    {
        #region Properties

        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public Category Category { get; set; }
        public string Description { get; set; }

        public double Price { get; set; }
        public string Image { get; set; }

        #endregion

        #region Constructors

        public ToolType(int id, string name, Category category, string description, double price, string image) 
        {
            Id = id;
            Name = name;
            Category = category;
            Description = description;
            Price = price;
            Image = image;
        }

        public ToolType() { }

        #endregion

        #region Methods

        //TODO

        #endregion
    }
}