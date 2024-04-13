using System.ComponentModel.DataAnnotations;

namespace Tic.Shared.Entites
{
    public class TicketInactive
    {
        public int TicketInactiveId { get; set; }

        [MaxLength(25, ErrorMessage = "El Maximo de caracteres es {0}")]
        [Required(ErrorMessage = "El campo {0} es Requerido")]
        [Display(Name = "Inactivo")]
        public string? Tiempo { get; set; }

        [Display(Name = "Orden")]
        public int? Orden { get; set; }

        [Display(Name = "Activo")]
        public bool Activo { get; set; }

    }
}
