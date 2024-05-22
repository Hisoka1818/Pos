using System.ComponentModel.DataAnnotations;

namespace Pos.Web.DTOs
{
    public class AccountUserDTO
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

        [Display(Name = "Teléfono")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string PhoneNumber { get; set; } = null!;

        public string Email { get; set; }
    }
}
