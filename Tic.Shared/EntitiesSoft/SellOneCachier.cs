using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tic.Shared.Entites;

namespace Tic.Shared.EntitiesSoft
{
    public class SellOneCachier
    {
        public int SellOneCachierId { get; set; }

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
        public int CachierId { get; set; }

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

        [Required(ErrorMessage = "La {0} es Obligatorio")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe Seleccionar un {0}")]
        [Display(Name = "Servidor")]
        public int OrderTicketDetailId { get; set; }

        //Propiedades No Mapeadas para manejo de Datos
        [NotMapped]
        public string? Usuario { get; set; }

        [NotMapped]
        public string? TextoHead { get; set; }

        [NotMapped]
        [Display(Name = "Inventario")]
        public int Stock { get; set; }
        //FIN........................................
        //...


        [Range(0, double.MaxValue, ErrorMessage = "El Valor del Precio debe ser mayor que {1}")]
        [Column(TypeName = "decimal(15,2)")]
        [Display(Name = "Tasa")]
        public decimal Rate { get; set; }

        [NotMapped]
        [Column(TypeName = "decimal(15,2)")]
        [Display(Name = "Precio")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "El campo {0} es Requerido")]
        [Range(1, double.MaxValue, ErrorMessage = "El Valor del Precio debe ser mayor que {1}")]
        [Display(Name = "Precio")]
        public decimal SubTotal { get; set; }

        [Required(ErrorMessage = "El campo {0} es Requerido")]
        [Range(0, double.MaxValue, ErrorMessage = "El Valor del Precio debe ser mayor que {1}")]
        [Display(Name = "Impuesto")]
        public decimal Impuesto { get; set; }

        [Required(ErrorMessage = "El campo {0} es Requerido")]
        [Range(1, double.MaxValue, ErrorMessage = "El Valor del Precio debe ser mayor que {1}")]
        [Display(Name = "Total")]
        public decimal Total { get; set; }


        //Manejo de Imagen de la empresa
        //para incrustar el logo el ticket
        [NotMapped]
        [Display(Name = "Imagen")]
        public string? ImageId { get; set; }

        //TODO: Pending to put the correct paths
        [Display(Name = "Imagen")]
        public string ImageFullPath => ImageId == string.Empty || ImageId == null
        ? $"https://localhost:7045/Images/NoImage.png"
        : $"https://localhost:7045/Images/ImgCorporate/{ImageId}";
        //? $"http://tickets.nexxtplanet.net/Images/NoImage.png"
        //: $"http://tickets.nexxtplanet.net/Images/ImgCorporate/{ImageId}";
        //Fin del manejo del logo de la empresa

        //En Caso que la venta se anule
        //.................................................
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha Cerrada")]
        public DateTime? DateAnulado { get; set; }

        [Display(Name = "Cerrada")]
        public bool Anulada { get; set; }


        //Solo Para armar Combos, para crear el Grupo de Servidor y filtrar los planes.
        [NotMapped]
        [Display(Name = "Grupo Servidor")]
        public int ServerGroupId { get; set; }

        [NotMapped]
        [Range(1, double.MaxValue, ErrorMessage = "Debe Seleccionar un {0}")]
        public IEnumerable<SelectListItem>? ListCategory { get; set; }

        [NotMapped]
        [Range(1, double.MaxValue, ErrorMessage = "Debe Seleccionar un {0}")]
        public IEnumerable<SelectListItem>? ListPlan { get; set; }

        [NotMapped]
        [Range(1, double.MaxValue, ErrorMessage = "Debe Seleccionar un {0}")]
        public IEnumerable<SelectListItem>? ListServer { get; set; }

        //Relacion de datos
        //...
        public int CorporateId { get; set; }

        public Corporate? Corporate { get; set; }

        public Cachier? Cachier { get; set; }

        public PlanCategory? PlanCategory { get; set; }

        public Plan? Plan { get; set; }

        public Server? Server { get; set; }

        public OrderTicketDetail? OrderTicketDetail { get; set; }

        ////..
        public ICollection<CachierPorcent>? CachierPorcents { get; set; }
    }
}
