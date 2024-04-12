using System.ComponentModel.DataAnnotations;

namespace Tic.Web.Models
{
    public class ChangePasswordViewModel
    {
        [Display(Name = "Clave Actual")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "The {0} field must contain between {2} and {1} characters.")]
        public string OldPassword { get; set; } = null!;

        [Display(Name = "Nueva Clave")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "The {0} field must contain between {2} and {1} characters.")]
        public string NewPassword { get; set; } = null!;

        [Display(Name = "Confirmar Clave")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "The {0} field must contain between {2} and {1} characters.")]
        [Compare("NewPassword")]
        public string Confirm { get; set; } = null!;
    }
}
