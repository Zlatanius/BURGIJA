using System;
using System.ComponentModel.DataAnnotations;

namespace Burgija.Models
{
    public class ToolType
    {
        #region Properties

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public Category Category { get; set; }
        public string Description { get; set; }

        #endregion

        #region Constructors

        public ToolType(int id, string name, Category category, string description) 
        {
            Id = id;
            Name = name;
            Category = category;
            Description = description;
        }

        public ToolType() { }

        #endregion

        #region Methods

        //TODO

        #endregion
    }
}