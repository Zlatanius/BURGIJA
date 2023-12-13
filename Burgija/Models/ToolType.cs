using System;
using System.ComponentModel.DataAnnotations;

namespace Burgija.Models
{
    public class ToolType : IComparable<ToolType>
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
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "Id mora biti veæi od 0.");
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Ime ne može biti null ili prazno.", nameof(name));
            }

            if (category == null)
            {
                throw new ArgumentNullException(nameof(category), "Kategorija ne može biti null.");
            }

            if (price < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(price), "Cijena ne može biti negativna.");
            }
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

        public int CompareTo(ToolType other)
        {
            return Price.CompareTo(other.Price);
        }

        #endregion
    }
}