using Microsoft.AspNetCore.Mvc.Rendering;

namespace Pos.Web.DTOs
{
    public class ProductDTOs
    {

        public int Id { get; set; }

        public string? Name { get; set; }

        public string? price { get; set; }

        public string? reference { get; set; }

        public string? area { get; set; }

        public int CategoriesId { get; set; }

        public IEnumerable<SelectListItem>? Categories { get; set; }
    }
}