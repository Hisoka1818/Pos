using System.ComponentModel.DataAnnotations;

namespace Pos.Web.Data.Entities
{
    public class PrivatePosRole
    {
        public int Id { get; set; }

        [Display(Name = "Rol")]
        [MaxLength(64, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; } = null!;

        public ICollection<RolePermission> RolePermissions { get; set; }
        public ICollection<RoleSection> RoleSections { get; set; }

        public IEnumerable<User> Users { get; set; }

    }
}
