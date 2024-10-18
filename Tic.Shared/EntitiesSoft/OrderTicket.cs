using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tic.Shared.Entites;

namespace Tic.Shared.EntitiesSoft
{
    public class OrderTicket
    {
        public int OrderTicketId { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Sistema de Control de Ordenes")]
        [Display(Name = "Control #")]
        public int OrdenControl { get; set; }

        [Required(ErrorMessage = "La {0} es Obligatorio")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "La {0} es Obligatorio")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe Seleccionar un {0}")]
        [Display(Name = "Categoria")]
        public int PlanCategoryId { get; set; }

        [Required(ErrorMessage = "La {0} es Obligatorio")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe Seleccionar un {0}")]
        [Display(Name = "Servidor")]
        public int ServerId { get; set; }

        [Required(ErrorMessage = "La {0} es Obligatorio")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe Seleccionar un {0}")]
        [Display(Name = "Plan")]
        public int PlanId { get; set; }

        //Registro del Nombre del Plan por Cambios en la Base de Datos
        [MaxLength(50, ErrorMessage = "El Maximo de caracteres es {1}")]
        [Display(Name = "Plan")]
        public string? NamePlan { get; set; }

        //Rate y Precio lo trae del Plan ///////////////////////////////////////////////////////
        //[Required(ErrorMessage = "El campo {0} es Requerido")]
        [Range(0, double.MaxValue, ErrorMessage = "El Valor del Precio debe ser mayor que {1}")]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Tasa")]
        public decimal Rate { get; set; }

        //Precio es el Valos con Impuesto ya
        [Required(ErrorMessage = "El campo {0} es Requerido")]
        [Range(1, double.MaxValue, ErrorMessage = "El Valor del Precio debe ser mayor que {1}")]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Precio")]
        public decimal Precio { get; set; }
        //Rate y Precio lo trae del Plan  /////////////////////////////////////////////////////

        //Precio o Valor del ticket sin Impuesto
        [Required(ErrorMessage = "El campo {0} es Requerido")]
        [Range(1, double.MaxValue, ErrorMessage = "El Valor del Precio debe ser mayor que {1}")]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Cant")]
        public decimal Cantidad { get; set; }

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
        [Display(Name = "Total")]
        public decimal Total { get; set; }


        //Total de Tickets Creados
        [Display(Name = "Total")]
        public int TotalTickets => OrderTicketDetails == null ? 0 : OrderTicketDetails.Count;

        [Display(Name = "Mk*")]
        public int TotalTicketMK => OrderTicketDetails == null ? 0 : OrderTicketDetails.Where(x=> x.MkId != null || x.MkId == string.Empty).Count();

        [Display(Name = "Mikrotik")]
        public bool? Mikrotik { get; set; }

        //Solo Para armar Combos
        [NotMapped]
        [Range(1, double.MaxValue, ErrorMessage = "Debe Seleccionar un {0}")]
        public IEnumerable<SelectListItem>? ListServer { get; set; }

        [NotMapped]
        [Range(1, double.MaxValue, ErrorMessage = "Debe Seleccionar un {0}")]
        public IEnumerable<SelectListItem>? ListCategory { get; set; }

        [NotMapped]
        [Range(1, double.MaxValue, ErrorMessage = "Debe Seleccionar un {0}")]
        public IEnumerable<SelectListItem>? ListPlan { get; set; }

        //...
        public int CorporateId { get; set; }

        public Corporate? Corporate { get; set; }

        public PlanCategory? PlanCategory { get; set; }

        public Plan? Plan { get; set; }

        public Server? Server { get; set; }

        [Display(Name = "Tickets Details")]
        public ICollection<OrderTicketDetail>? OrderTicketDetails { get; set; }
    }
}
