using System.ComponentModel.DataAnnotations;

namespace Tic.Web.Models
{
    public class LoginViewModel
    {
        [EmailAddress]
        [Required(ErrorMessage = "El campo {0} es Requerido")]
        [Display(Name = "Usuario")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "El campo {0} es Requerido")]
        [MinLength(6, ErrorMessage = "El Minimo de caracteres es de {1}")]
        [Display(Name = "Clave")]
        public string Password { get; set; } = null!;

        public bool RememberMe { get; set; }
    }
}
