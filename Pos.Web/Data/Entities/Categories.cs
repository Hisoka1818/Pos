using System.ComponentModel.DataAnnotations;

namespace Pos.Web.Data.Entities
{
    public class Categories
    {
        public int Id { get; set; }
        [Display(Name = "Nombre de la categoria")]
        public string categoryName { get; set; }
        [Display(Name = "Tipo de la categoria")]
        public string categoryDescription { get; set; }


    }
}
