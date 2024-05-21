using System.ComponentModel.DataAnnotations;

namespace Pos.Web.Data.Entities
{
    public class Product
    {
        public int Id { get; set; }

        [Display(Name = "Nombre" )]
        public string Name { get; set; }

        [Display(Name = "Precio")]
        public string price { get; set; }

        [Display(Name = "Referencias")]
        public string reference { get; set; }

        [Display(Name = "Area")]
        public string area { get; set; }

        public int CategoriesId { get; set; }

        //propiedades de navegación 
        [Display(Name = "Categoria")]
        public Categories Categories { get; set; }

        public List<SalesDetail> SalesDetails { get; set; }

    }
}
        