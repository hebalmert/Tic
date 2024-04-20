using System.ComponentModel.DataAnnotations;

namespace Tic.Shared.ReportesDTOs
{
    public class RepCachierDTOs
    {
        [Required(ErrorMessage = "La {0} es Obligatorio")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe Seleccionar un {0}")]
        [Display(Name = "Compania")]
        public int CorporateId { get; set; }

        [Required(ErrorMessage = "La {0} es Obligatorio")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fechab Inicio")]
        public DateTime DateInicio { get; set; }

        [Required(ErrorMessage = "La {0} es Obligatorio")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fechab Fin")]
        public DateTime DateFin { get; set; }

        [Required(ErrorMessage = "La {0} es Obligatorio")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe Seleccionar un {0}")]
        [Display(Name = "Cajero")]
        public int CachierId { get; set; }
    }
}
