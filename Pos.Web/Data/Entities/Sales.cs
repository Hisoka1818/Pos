using System.ComponentModel.DataAnnotations;

namespace Pos.Web.Data.Entities
{
    public class Sales
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
        public string? SalesType { get; set;}

        public int CustomerId { get; set; }

        //propiedad de navegacion
        [Display(Name = "Cliente")]
        public Customer Customer { get; set; }

        //public List<SalesDetail> SalesDetails { get; set; }
    }
}
