using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tic.Shared.Entites;

namespace Tic.Shared.EntitiesSoft
{
    public class SellPack
    {
        [Key]
        public int SellPackId { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Debe Seleccionar un {0}")]
        [Display(Name = "Venta #")]
        public int SellControl { get; set; }

        [Required(ErrorMessage = "La {0} es Obligatorio")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "La {0} es Obligatorio")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe Seleccionar un {0}")]
        [Display(Name = "Usuario")]
        public int ManagerId { get; set; }

        [Required(ErrorMessage = "La {0} es Obligatorio")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe Seleccionar un {0}")]
        [Display(Name = "Categoria")]
        public int PlanCategoryId { get; set; }

        [Required(ErrorMessage = "La {0} es Obligatorio")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe Seleccionar un {0}")]
        [Display(Name = "Plan")]
        public int PlanId { get; set; }

        [MaxLength(50, ErrorMessage = "El Maximo de caracteres es {1}")]
        [Display(Name = "Plan")]
        public string? NamePlan { get; set; }

        [Required(ErrorMessage = "La {0} es Obligatorio")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe Seleccionar un {0}")]
        [Display(Name = "Servidor")]
        public int ServerId { get; set; }

        //Propiedades No Mapeadas para manejo de Datos
        [NotMapped]
        public string? TextoHead { get; set; }

        [NotMapped]
        [Display(Name = "Inventario")]
        public int Stock { get; set; }
        //FIN........................................
        //...

        [Range(1, double.MaxValue, ErrorMessage = "El Valor del Precio debe ser mayor que {1}")]
        [Display(Name = "Cantidad")]
        public decimal Cantidad { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "El Valor del Precio debe ser mayor que {1}")]
        [Display(Name = "Tasa")]
        public decimal Rate { get; set; }

        [Required(ErrorMessage = "El campo {0} es Requerido")]
        [Range(1, double.MaxValue, ErrorMessage = "El Valor del Precio debe ser mayor que {1}")]
        [Display(Name = "Precio")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "El campo {0} es Requerido")]
        [Range(1, double.MaxValue, ErrorMessage = "El Valor del Precio debe ser mayor que {1}")]
        [Display(Name = "SubTotal")]
        public decimal SubTotal { get; set; }

        [Required(ErrorMessage = "El campo {0} es Requerido")]
        [Range(0, double.MaxValue, ErrorMessage = "El Valor del Impuesto debe ser mayor que {1}")]
        [Display(Name = "Impuesto")]
        public decimal Impuesto { get; set; }

        [Required(ErrorMessage = "El campo {0} es Requerido")]
        [Range(1, double.MaxValue, ErrorMessage = "El Valor del Precio debe ser mayor que {1}")]
        [Display(Name = "Total")]
        public decimal Total { get; set; }


        //Total de Tickets SellPackDetails
        [Display(Name = "Creados")]
        public int TotalTickets => SellPackDetails == null ? 0 : SellPackDetails.Count;


        //Para Cierre de la SellPack
        //.................................................
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha Cerrada")]
        public DateTime? DateClose { get; set; }

        [Display(Name = "Cerrada")]
        public bool Closed { get; set; }
        //Fin de Cierre
        //...

        //Solo Para armar Combos, para crear el Grupo de Servidor y filtrar los planes.
        [NotMapped]
        [Display(Name = "Grupo Servidor")]
        public int ServerGroupId { get; set; }

        [NotMapped]
        [Range(1, double.MaxValue, ErrorMessage = "Debe Seleccionar un {0}")]
        public IEnumerable<SelectListItem>? ListCategory { get; set; }

        [NotMapped]
        [Range(1, double.MaxValue, ErrorMessage = "Debe Seleccionar un {0}")]
        public IEnumerable<SelectListItem>? ListServer { get; set; }

        [NotMapped]
        [Range(1, double.MaxValue, ErrorMessage = "Debe Seleccionar un {0}")]
        public IEnumerable<SelectListItem>? ListPlan { get; set; }


        //...
        public int CorporateId { get; set; }

        public Corporate? Corporate { get; set; }

        public Manager? Manager { get; set; }

        public PlanCategory? PlanCategory { get; set; }

        public Plan? Plan { get; set; }

        public Server? Server { get; set; }

        //..
        [Display(Name = "Tickets Details")]
        public ICollection<SellPackDetail>? SellPackDetails { get; set; }
    }
}
