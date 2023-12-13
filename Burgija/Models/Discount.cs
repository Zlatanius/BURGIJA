using System;
using System.ComponentModel.DataAnnotations;

namespace Burgija.Models
{
    public class Discount
    {
        #region Properties

        [Key]
        public int Id { get; set; }
        public DateTime StartOfDiscount { get; set; }
        public DateTime EndOfDiscount { get; set; }
        public double Percent { get; set; }

        #endregion

        #region Constructors

        public Discount(int id, DateTime startOfDiscount, DateTime endOfDiscount, double percent)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "Id mora biti veæi od 0.");
            }

            if (startOfDiscount >= endOfDiscount)
            {
                throw new ArgumentException("Datum poèetka popusta mora biti prije datuma kraja popusta.");
            }
            if (percent < 0 || percent>100)
            {
                throw new ArgumentOutOfRangeException(nameof(percent), "Procenat mora biti izmeðu 0 i 100.");
            }
            Id =id;
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