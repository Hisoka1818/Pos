using System.ComponentModel.DataAnnotations;

namespace Pos.Web.DTOs
{
    public class PrivatePosRoleDTO
    {
        public int Id { get; set; }

        [Display(Name = "Rol")]
        [MaxLength(64, ErrorMessage = "El campo {0} debe tener m�ximo {1} caract�res.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; }

        public List<PermissionForDTO>? Permissions { get; set; }

        public string? PermissionIds { get; set; }

        public List<SectionForDTO>? Sections { get; set; }

        public string? SectionIds { get; set; }
    }
}