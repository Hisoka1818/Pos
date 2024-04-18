using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Pos.Web.DTOs
{
    public class SalesDTO
    {
        public int Id { get; set; }
        [Display(Name = "Fecha y Hora")]
        public DateTime DateSales { get; set; }

        [Display(Name = "Descuento")]
        public decimal DiscountsSales { get; set; }

        [Display(Name = "Valor total")]
        public decimal TotalSales { get; set; }

        [Display(Name = "Metodo de pago")]
        public string PaymentMethod { get; set; }

        [Display(Name = "Tipo de venta")]
        public string? SalesType { get; set; }

        [Display(Name = "Cliente")]
        public int CustomerId { get; set; }

        public IEnumerable<SelectListItem>? Customer { get; set; }
    }
}
