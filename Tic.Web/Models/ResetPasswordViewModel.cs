using System.ComponentModel.DataAnnotations;

namespace Tic.Web.Models
{
    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string UserName { get; set; } = null!;

        [Required]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "El {0} campo debe contener entre {2} y {1} Caracteres.")]
        [DataType(DataType.Password)]
        [Display(Name = "Nueva Clave")]
        public string Password { get; set; } = null!;

        [Required]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "El {0} campo debe contener entre {2} y {1} Caracteres.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Clave")]
        [Compare("Password")]
        public string ConfirmPassword { get; set; } = null!;

        [Required]
        public string Token { get; set; } = null!;
    }
}
