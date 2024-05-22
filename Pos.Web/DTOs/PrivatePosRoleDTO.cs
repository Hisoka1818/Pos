using System.ComponentModel.DataAnnotations;

namespace Pos.Web.DTOs
{
    public class PrivatePosRoleDTO
    {
        public int Id{get;set;}

        [Display(Name = "Rol")]
        [MaxLength(64, ErrorMessage ="El campo {0} debe tener maximo {1} caracteres")]
        [Required(ErrorMessage ="El campo {0} es obligatorio")]

        public string Name {get;set;}

        public List<PermissionForDTO>? permissions{get;set;}

        public string? PermissionIds {get;set;}    
    }
}