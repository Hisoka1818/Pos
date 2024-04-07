using Microsoft.AspNetCore.Mvc.Rendering;

namespace Pos.Web.DTOs
{
    public class SalesDTO
    {
        public int Id { get; set; }

        public DateTime DateSales { get; set; }

        public decimal DiscountsSales { get; set; }

        public decimal TotalSales { get; set; }

        public string PaymentMethod { get; set; }

        public string? SalesType { get; set; }

        public int CustomerId { get; set; }

        public IEnumerable<SelectListItem>? Customer { get; set; }
    }
}
