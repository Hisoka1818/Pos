using System.ComponentModel.DataAnnotations;

namespace Pos.Web.DTOs
{
    public class ChangePasswordDTO
    {
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña actual")]
        [MinLength(4, ErrorMessage = "El campo '{0}' debe tener una longitud mínima de {1} carácteres")]
        [Required(ErrorMessage = "El campo '{0}' es requerido.")]
        public string CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Nueva contraseña")]
        [MinLength(4, ErrorMessage = "El campo '{0}' debe tener una longitud mínima de {1} carácteres")]
        [Required(ErrorMessage = "El campo '{0}' es requerido.")]
        public string NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "La nueva contraseña y la confirmación no son iguales")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmación de contraseña")]
        [MinLength(4, ErrorMessage = "El campo '{0}' debe tener una longitud mínima de {1} carácteres")]
        [Required(ErrorMessage = "El campo '{0}' es requerido.")]
        public string ConfirmPassword { get; set; }
    }
}
