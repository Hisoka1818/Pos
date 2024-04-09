

using System.ComponentModel.DataAnnotations;

namespace Pos.Web.Data.Entities
{
    public class Customer
    {
        public int Id { get; set; }

        [Display(Name = "Nombre")]
        public string FirstName { get; set; }

        [Display(Name = "Apellido")]
        public string LastName { get; set;}

        [Display(Name = "Numero de telefono")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Email")]
        public string EmailAddress { get; set; }

        [Display(Name = "Tipo de cliente")]
        public string CustomerType { get; set;}

        //propiedad de navegacion

    }
}
