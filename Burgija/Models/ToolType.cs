using System;

namespace Burgija.Models
{
    public class ToolType
    {
        #region Attributes

        private int id;
        private string name;
        private Category category;
        private string description;

        #endregion

        #region Properties

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public Category Category { get => category; set => category = value; }
        public string Description { get => description; set => description = value; }

        #endregion

        #region Constructor

        public ToolType(int id, string name, Category category, string description) 
        {
            Id = id;
            Name = name;
            Category = category;
            Description = description;
        }

        #endregion

        #region Methods

        //TODO

        #endregion
    }
}