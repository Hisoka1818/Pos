namespace Pos.Web.Data.Entities
{
    public class Sales
    {
        public int Id { get; set; }

        public DateTime DateSales { get; set; }

        public decimal DiscountsSales { get; set; }

        public decimal TotalSales { get; set; }

        public string? PaymentMethod { get; set; }

        public string? SalesType { get; set;}

        public List<SalesDetail> SalesDetails { get; set; }

    }
}
