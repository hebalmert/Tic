using Microsoft.AspNetCore.Mvc.Rendering;
using Tic.Shared.Entites;
using Tic.Shared.Enum;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tic.Shared.EntitiesSoft
{
    public class Plan
    {
        [Key]
        public int PlanId { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = false)]
        [Display(Name = "Creado")]
        public DateTime? DateCreated { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = false)]
        [Display(Name = "Editado")]
        public DateTime? DateEdit { get; set; }

        [Required(ErrorMessage = "La {0} es Obligatorio")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe Seleccionar un {0}")]
        [Display(Name = "Servidor")]
        public int ServerId { get; set; }

        [Required(ErrorMessage = "La {0} es Obligatorio")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe Seleccionar un {0}")]
        [DisplayName("Categoria")]
        public int PlanCategoryId { get; set; }

        [Display(Name = "Plan")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string PlanName { get; set; } = null!;

        [Required(ErrorMessage = "La {0} es Obligatorio")]
        [Range(1, double.MaxValue, ErrorMessage = "El Valor de {0} debe ser mayor a {1}")]
        [Display(Name = "UpLoad")]
        public int SpeedUp { get; set; }

        [Display(Name = "")]
        public SpeedUpType SpeedUpType { get; set; }

        [Required(ErrorMessage = "La {0} es Obligatorio")]
        [Range(1, double.MaxValue, ErrorMessage = "El Valor de {0} debe ser mayor a {1}")]
        [Display(Name = "Download")]
        public int SpeedDown { get; set; }


        [Display(Name = "")]
        public SpeedDownType SpeedDownType { get; set; }

        //Los tiempos de Tickets, Inactividad y Refrescamiento de los tickets.
        [Required(ErrorMessage = "La {0} es Obligatorio")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe Seleccionar un {0}")]
        [Display(Name = "Tiempo Ticket")]
        public int TicketTimeId { get; set; }

        [Required(ErrorMessage = "La {0} es Obligatorio")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe Seleccionar un {0}")]
        [Display(Name = "Inactividad")]
        public int TicketInactiveId { get; set; }

        [Required(ErrorMessage = "La {0} es Obligatorio")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe Seleccionar un {0}")]
        [Display(Name = "Refrescamiento")]
        public int TicketRefreshId { get; set; }

        //Para compartir y tipo de consumo del ticket
        [Required(ErrorMessage = "La {0} es Obligatorio")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe Seleccionar un {0}")]
        [Display(Name = "Usuarios")]
        public int ShareUser { get; set; }

        [Display(Name = "Excluir Proxy")]
        public bool Proxy { get; set; }

        [Display(Name = "Mac Cookies")]
        public bool MacCookies { get; set; }

        [Display(Name = "Continuo")]
        public bool ContinueTime { get; set; }


        //Precio o Valor del Servicio sin Impuesto
        //...//...//...//...//

        [Required(ErrorMessage = "La {0} es Obligatorio")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe Seleccionar un {0}")]
        [Display(Name = "Impuesto")]
        public int TaxId { get; set; }

        //[Required(ErrorMessage = "El campo {0} es Requerido")]
        [Range(0, double.MaxValue, ErrorMessage = "El Valor del Precio debe ser mayor que {1}")]
        [Column(TypeName = "decimal(18,2)")]
        [DisplayFormat(DataFormatString = "{0:n2}")]
        [Display(Name = "SubTotal")]
        public decimal SubTotal { get; set; }

        //[Required(ErrorMessage = "El campo {0} es Requerido")]
        [Range(0, double.MaxValue, ErrorMessage = "El Valor del Precio debe ser mayor que {1}")]
        [Column(TypeName = "decimal(18,2)")]
        [DisplayFormat(DataFormatString = "{0:n2}")]
        [Display(Name = "Impuesto")]
        public decimal Impuesto { get; set; }


        [Required(ErrorMessage = "El campo {0} es Requerido")]
        [Range(0, double.MaxValue, ErrorMessage = "El Valor del Precio debe ser mayor que {1}")]
        [Column(TypeName = "decimal(18,2)")]
        [DisplayFormat(DataFormatString = "{0:n2}")]
        [Display(Name = "Precio")]
        public decimal Precio { get; set; }
        //...//...//...//...//...//
        //Niveles de Precio FIN

        [Display(Name = "Activo")]
        public bool Active { get; set; }

        [MaxLength(15, ErrorMessage = " El Campo {0} debe ser menor de {1} Caracteres")]
        [Display(Name = "Mk-Id")]
        public string? MkId { get; set; }

        [MaxLength(15, ErrorMessage = " El Campo {0} debe ser menor de {1} Caracteres")]
        [Display(Name = "MkConsumo-Id")]
        public string? MkContinuoId { get; set; }

        //Propiedades NoMapped para manejo de Combos
        [NotMapped]
        public IEnumerable<SelectListItem>? ListUp { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem>? ListDown { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem>? ListTax { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem>? ListCatPlan { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem>? ListServer { get; set; }


        [NotMapped]
        public IEnumerable<SelectListItem>? ListTimeInactive { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem>? ListTimeRefresh { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem>? ListTimeTicket { get; set; }

        //Pripiedades Virtuales para Manejo de Propiedades del Modelo

        //[NotMapped]
        public decimal? PrecioconImpuesto => Math.Round((((Precio * RateTax) / 100) + Precio),2);

        [NotMapped]
        public decimal RateTax => Tax == null ? 0 : Tax!.Rate;



        [NotMapped]
        public string VelocidadDown => Convert.ToString(SpeedDown) + SpeedDownType;

        [NotMapped]
        public string VelocidadUp => Convert.ToString(SpeedUp) + SpeedUpType;

        [NotMapped]
        [Display(Name = "Up / Down")]
        public string VelocidadTotal => $"{VelocidadUp}/{VelocidadDown}";

        //Relaciones de Tablas

        //A que Corporacion Pertenece
        public int CorporateId { get; set; }

        public Corporate? Corporate { get; set; }

        public PlanCategory? PlanCategory { get; set; }

        public Server? Server { get; set; }

        public Tax? Tax { get; set; }

        public TicketInactive? TicketInactive { get; set; }

        public TicketRefresh? TicketRefresh { get; set; }

        public TicketTime? TicketTime { get; set; }

    }
}
