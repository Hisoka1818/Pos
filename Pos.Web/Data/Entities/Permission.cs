using System.ComponentModel.DataAnnotations;

namespace Pos.Web.Data.Entities
{
    public class Permission
    {
        public int Id { get; set; }

        [Display(Name = "Permiso")]
        [MaxLength(64, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; } = null!;

        [Display(Name = "Descripción")]
        [MaxLength(512, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        public string? Description { get; set; }

        [Display(Name = "Módulo")]
        [MaxLength(64, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Module { get; set; } = null!;

        public ICollection<RolePermission> RolePermissions { get; set; } 
    }
}
