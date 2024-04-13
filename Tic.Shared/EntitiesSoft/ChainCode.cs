using System.ComponentModel.DataAnnotations;
using Tic.Shared.Entites;

namespace Tic.Shared.EntitiesSoft
{
    public class ChainCode
    {
        public int ChainCodeId { get; set; }

        [Required(ErrorMessage = "La {0} es Obligatorio")]
        [MaxLength(36, ErrorMessage = "El Maximo de caracteres es {1}")]
        [Display(Name = "Cadena")]
        public string? Cadena { get; set; }

        [Required(ErrorMessage = "La {0} es Obligatorio")]
        [Range(1, 16, ErrorMessage = "Debe Seleccionar un {0}")]
        [Display(Name = "Largo Ticket")]
        public int Largo { get; set; }


        //...
        public int CorporateId { get; set; }

        public Corporate? Corporate { get; set; }
    }
}
