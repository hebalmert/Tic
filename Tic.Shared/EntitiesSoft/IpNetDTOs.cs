using System.ComponentModel.DataAnnotations;

namespace Tic.Shared.EntitiesSoft
{
    public class IpNetDTOs
    {

        [Required(ErrorMessage = "El Campo {0} es Requerido")]
        [MaxLength(50, ErrorMessage = " El Campo {0} debe ser menor de {1} Caracteres")]
        [RegularExpression(@"^(?:[0-9]{1,3}\.){2}[0-9]{1,3}$", ErrorMessage = "El Campo {0} tener el Formato ejm: 192.168.11")]
        [Display(Name = "Ip Address")]
        public string? Ip1 { get; set; }

        [Required(ErrorMessage = "El Campo {0} es Requerido")]
        [Range(1, 255, ErrorMessage = "El Rango debe estar entre {1} y {2}")]
        [Display(Name = "Desde")]
        public int Desde { get; set; }

        [Required(ErrorMessage = "El Campo {0} es Requerido")]
        [Range(1, 255, ErrorMessage = "El Rango debe estar entre {1} y {2}")]
        [Display(Name = "Hasta")]
        public int Hasta { get; set; }

        [Required(ErrorMessage = "El Campo {0} es Requerido")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe Seleccionar un {0}")]
        [Display(Name = "Corporacion")]
        public int CorporateId { get; set; }
    }
}
