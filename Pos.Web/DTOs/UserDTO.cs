using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Pos.Web.DTOs
{
    public class UserDTO
    {
        public Guid Id { get; set; }

        [Display(Name = "Documento")]
        [MaxLength(32, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Document { get; set; } = null!;

        [Display(Name = "Nombres")]
        [MaxLength(64, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string FirstName { get; set; } = null!;

        [Display(Name = "Apellidos")]
        [MaxLength(64, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string LastName { get; set; } = null!;

        public bool IsNew { get; set; }

        [Display(Name = "Rol")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un rol.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int PrivateBlogRoleId { get; set; }

        public IEnumerable<SelectListItem>? PrivatePosRoles { get; set; }

        [Display(Name = "Teléfono")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string PhoneNumber { get; set; } = null!;

        [Display(Name = "Email")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [EmailAddress(ErrorMessage = "El campo {0} debe ser un Email válido")]
        public string Email { get; set; } = null!;

        [Display(Name = "Usuario")]
        public string FullName => $"{FirstName} {LastName}";
    }
}
