using System;

namespace Burgija.Models
{
    public class Discount
    {
        #region Attributes

        private int id;
        private DateTime startOfDiscount;
        private DateTime endOfDiscount;
        private double percent;

        #endregion

        #region Properties

        public int Id { get => id; set => id = value; } 
        public DateTime StartOfDiscount { get => startOfDiscount; set => startOfDiscount = value; }
        public DateTime EndOfDiscount { get => endOfDiscount; set => endOfDiscount = value; }
        public double Percent { get => percent; set => percent = value; }

        #endregion

        #region Constructor

        public Discount(int id, DateTime startOfDiscount, DateTime endOfDiscount, double percent)
        {
            Id=id;
            StartOfDiscount=startOfDiscount;
            EndOfDiscount=endOfDiscount;
            Percent=percent;
        }

        #endregion

        #region Methods

        //TODO

        #endregion
    }
}