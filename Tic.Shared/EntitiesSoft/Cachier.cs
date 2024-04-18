using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tic.Shared.Entites;
using Tic.Shared.Enum;

namespace Tic.Shared.EntitiesSoft
{
    public class Cachier
    {
        public int CachierId { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Creado")]
        public DateTime? DateCreated { get; set; }

        [Display(Name = "Foto")]
        public string? Photo { get; set; }

        [MaxLength(50, ErrorMessage = "El campo no puede ser mayor a {0} de largo")]
        [Required]
        [Display(Name = "Nombre")]
        public string? FirstName { get; set; }

        [MaxLength(50, ErrorMessage = "El campo no puede ser mayor a {0} de largo")]
        [Required]
        [Display(Name = "Apellido")]
        public string? LastName { get; set; }

        [MaxLength(100, ErrorMessage = "El campo no puede ser mayor a {0} de largo")]
        [Display(Name = "Cajero")]
        public string? FullName { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = "Debe Seleccionar un Item")]
        [Required(ErrorMessage = "El {0} es Obligatorio")]
        [Display(Name = "Tipo")]
        public int DocumentTypeId { get; set; }

        [MaxLength(25, ErrorMessage = "El campo no puede ser mayor a {0} de largo")]
        [Required(ErrorMessage = "El {0} es Obligatorio")]
        [Display(Name = "Documento")]
        public string? Document { get; set; }

        [Required(ErrorMessage = "El {0} es Obligatorio")]
        [MaxLength(256, ErrorMessage = "El campo no puede ser mayor a {0} de largo")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string? UserName { get; set; }

        [MaxLength(25, ErrorMessage = "El campo no puede ser mayor a {0} de largo")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Telefono")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "El {0} es Obligatorio")]
        [MaxLength(256, ErrorMessage = "El campo no puede ser mayor a {0} de largo")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Direccion")]
        public string? Address { get; set; }

        [Display(Name = "Tipo Usuario")]
        public UserType UserType { get; set; }

        [Display(Name = "Activo")]
        public bool Activo { get; set; }

        [NotMapped]
        [Display(Name = "Imagen")]
        public IFormFile? ImageFile { get; set; }

        //TODO: Pending to put the correct paths
        [Display(Name = "Imagen")]
        public string ImageFullPath => Photo == string.Empty || Photo == null
        ? $"https://localhost:7045/Images/NoImage.png"
        : $"https://localhost:7045/Images/ImgCachier/{Photo}";
        //? $"http://tickets.nexxtplanet.net/Images/NoImage.png"
        //: $"http://tickets.nexxtplanet.net/Images/ImgCachier/{ImageId}";


        //Manejo del Cajero en Asignacion de Servidores
        [Display(Name = "Multi Servidor")]
        public bool MultiServer { get; set; }

        //[Range(1, double.MaxValue, ErrorMessage = "Debe Seleccionar un {0}")]
        [Display(Name = "Servidores")]
        public int? ServerId { get; set; }
        //Fin ...........................................


        //Si Posee Comision por la venta de Ticket el Cajero
        [Display(Name = "Sin Porcentaje")]
        public bool Porcentaje { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "El Valor del Precio debe ser mayor que {1}")]
        [Column(TypeName = "decimal(15,2)")]
        [Display(Name = "Porcentaje Venta")]
        public decimal RateCachier { get; set; }
        //Fin....................................................


        //...
        public int CorporateId { get; set; }

        public Corporate? Corporate { get; set; }

        //...
        public DocumentType? DocumentType { get; set; }

        //...
        public Server? Server { get; set; }

        //Propiedades NoMapped para manejo de Combos
        [NotMapped]
        public IEnumerable<SelectListItem>? ListDocument { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem>? ListServer { get; set; }

        ////..
        //[Display(Name = "Cajero")]
        //public ICollection<SellOneCachier> SellOneCachiers { get; set; }

        ////..
        //[Display(Name = "Comisiones Cajero")]
        //public ICollection<CachierPorcent> CachierPorcents { get; set; }
    }
}
