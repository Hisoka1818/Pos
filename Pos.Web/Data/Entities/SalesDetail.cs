namespace Pos.Web.Data.Entities
{
    public class SalesDetail
    {
        public int SalesId { get; set; }

        public int ProductId { get; set; }

        public int QuantitySold { get; set; }

        public decimal Subtotal { get; set; }
    }
}
