using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Pos.Web.DTOs
{
    public class ProductDTOs
    {

        public int Id { get; set; }
        [Display(Name = "Nombre")]
        public string? Name { get; set; }

        [Display(Name = "Precio")]
        public string? price { get; set; }

        [Display(Name = "Referencias")]
        public string? reference { get; set; }

        [Display(Name = "Area")]
        public string? area { get; set; }

        [Display(Name = "Categoria")]
        public int CategoriesId { get; set; }

        public IEnumerable<SelectListItem>? Categories { get; set; }
    }
}