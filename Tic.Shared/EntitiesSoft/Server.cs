using Microsoft.AspNetCore.Mvc.Rendering;
using Tic.Shared.Entites;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tic.Shared.EntitiesSoft
{
    public class Server
    {
        [Key]
        public int ServerId { get; set; }

        [Display(Name = "Servidor")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string ServerName { get; set; } = null!;

        [Required(ErrorMessage = "La {0} es Obligatorio")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe Seleccionar un {0}")]
        [DisplayName("Ip Network")]
        public int IpNetworkId { get; set; }

        [Display(Name = "Usuario")]
        [MaxLength(25, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Usuario { get; set; } = null!;

        [Display(Name = "Clave")]
        [MaxLength(25, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Clave { get; set; } = null!;

        [NotMapped]
        [Display(Name = "Clave")]
        [MaxLength(25, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Compare("Clave", ErrorMessage = "La Clave no Coincide, Verifique")]
        public string ClaveConfirm { get; set; } = null!;

        [Display(Name = "Wan Name")]
        [MaxLength(25, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        //[Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string WanName { get; set; } = null!;

        [Range(1, double.MaxValue, ErrorMessage = "Debe Seleccionar un {0}")]
        [DisplayName("Puerto API")]
        public int ApiPort { get; set; }

        [Required(ErrorMessage = "La {0} es Obligatorio")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe Seleccionar un {0}")]
        [DisplayName("Marca")]
        public int MarkId { get; set; }

        [Required(ErrorMessage = "La {0} es Obligatorio")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe Seleccionar un {0}")]
        [DisplayName("Modelo")]
        public int MarkModelId { get; set; }

        [Required(ErrorMessage = "La {0} es Obligatorio")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe Seleccionar un {0}")]
        [DisplayName("Zona")]
        public int ZoneId { get; set; }

        [Display(Name = "Activo")]
        public bool Active { get; set; }


        //A que Corporacion Pertenece
        public int CorporateId { get; set; }

        public Corporate? Corporate { get; set; }

        //Relaciones con los campos Id de otros modelos
        public IpNetwork? IpNetwork { get; set; }

        public Mark? Mark { get; set; }

        public MarkModel? MarkModel { get; set; }

        public Zone? Zone { get; set; }

        public ICollection<Plan>? Plans { get; set; }


        //Propiedades de Solo Control y Consultas de datos
        //No mapeadas a la Base de datos

        //Armado de Combos
        [NotMapped]
        public int? CurrentIpNetworkId { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem>? ListIpNetwork { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem>? ListMark { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem>? ListMarkModel { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem>? ListState { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem>? ListCity { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem>? ListZone { get; set; }


        //Conjunto de seleccion de Zona apartir de la Estado y Ciudad
        [DisplayName("Estado")]
        [NotMapped]
        public int StateId { get; set; }

        [DisplayName("Ciudad")]
        [NotMapped]
        public int CityId { get; set; }



    }
}
