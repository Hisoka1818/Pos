using System.ComponentModel.DataAnnotations;

namespace Pos.Web.Data.Entities
{
    public class Section
    {
        public int Id { get; set; }
        [Display(Name="Seccion")]
        [Required(ErrorMessage = "Elcampo '{0}' es requerido.")]
        [MaxLength(64, ErrorMessage = "El campo '{0}' debe tener maximo {1} caracteres")]
        public string Name {  get; set; } 

        [Display(Name = "¿Esta oculta?")]
        public bool IsHidden { get; set; } = false;
        public ICollection<RoleSection>RoleSections { get; set; }
    }
}
