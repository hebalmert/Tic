using System.ComponentModel.DataAnnotations;

namespace Tic.Shared.Entites
{
    public class TicketTime
    {
        public int TicketTimeId { get; set; }

        [MaxLength(25, ErrorMessage = "El Maximo de caracteres es {0}")]
        [Required(ErrorMessage = "El campo {0} es Requerido")]
        [Display(Name = "Inactivo")]
        public string? Tiempo { get; set; }

        [Display(Name = "Orden")]
        public int? Orden { get; set; }

        [Display(Name = "Activo")]
        public bool Activo { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Script Consumo")]
        public string? ScriptConsumo { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Script Continuo")]
        public string? ScriptContinuo { get; set; }

    }
}
