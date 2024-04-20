using System.ComponentModel.DataAnnotations;
using Tic.Shared.Entites;

namespace Tic.Shared.EntitiesSoft
{
    public class CachierPorcent
    {
        public int CachierPorcentId { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha")]
        public DateTime? Date { get; set; }

        //ID del Cajero
        public int CachierId { get; set; }

        //Id de la Venta Cachier
        public int SellOneCachierId { get; set; }

        //Para saber el ID del ticket vendido
        public int OrderTicketDetailId { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "El Valor del Precio debe ser mayor que {1}")]
        [Display(Name = "Porcentaje Venta")]
        public decimal Porcentaje { get; set; }

        //Nombre Plan del ticket
        [MaxLength(50, ErrorMessage = "El Maximo de caracteres es {1}")]
        [Display(Name = "Plan")]
        public string? NamePlan { get; set; }

        [Required(ErrorMessage = "El campo {0} es Requerido")]
        [Range(1, double.MaxValue, ErrorMessage = "El Valor del Precio debe ser mayor que {1}")]
        [Display(Name = "Precio Sin Impuesto")]
        public decimal Precio { get; set; }


        [Required(ErrorMessage = "El campo {0} es Requerido")]
        [Range(1, double.MaxValue, ErrorMessage = "El Valor del Precio debe ser mayor que {1}")]
        [Display(Name = "Precio Sin Impuesto")]
        public decimal Comision { get; set; }


        ///Para manejar datos del pago de cada Comision
        ///
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha Pago")]
        public DateTime? DatePagado { get; set; }

        [Required(ErrorMessage = "La {0} es Obligatorio")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe Seleccionar un {0}")]
        [Display(Name = "Pago #")]
        public int Control { get; set; }

        [Display(Name = "Comision Pagada")]
        public bool Pagado { get; set; }


        //...
        public int CorporateId { get; set; }

        public Corporate? Corporate { get; set; }

        public Cachier? Cachier { get; set; }

        public SellOneCachier? SellOneCachier { get; set; }

        public OrderTicketDetail? OrderTicketDetail { get; set; }
    }
}
