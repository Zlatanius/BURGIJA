using System;

namespace Burgija.Models
{
    public class Discount
    {
        #region Properties

        public int Id { get; set; }
        public DateTime StartOfDiscount { get; set; }
        public DateTime EndOfDiscount { get; set; }
        public double Percent { get; set; }

        #endregion

        #region Constructors

        public Discount(int id, DateTime startOfDiscount, DateTime endOfDiscount, double percent)
        {
            Id=id;
            StartOfDiscount=startOfDiscount;
            EndOfDiscount=endOfDiscount;
            Percent=percent;
        }

        public Discount() { }

        #endregion

        #region Methods

        //TODO

        #endregion
    }
}