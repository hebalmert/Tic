using System.ComponentModel.DataAnnotations;

namespace Tic.Web.Models
{
    public class RecoverPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Correo")]
        public string Email { get; set; } = null!;
    }
}
