using Tic.Shared.Entites;
using System.ComponentModel.DataAnnotations;

namespace Tic.Shared.EntitiesSoft
{
    public class IpNetwork
    {
        public int IpNetworkId { get; set; }

        [Required(ErrorMessage = "El Campo {0} es Requerido")]
        [MaxLength(50, ErrorMessage = " El Campo {0} debe ser menor de {1} Caracteres")]
        [RegularExpression(@"^(?:[0-9]{1,3}\.){3}[0-9]{1,3}$", ErrorMessage = "El Campo {0} tener el Formato Ejm: 192.168.0.0")]
        [Display(Name = "IP Address")]
        public string? Ip { get; set; }

        [MaxLength(250, ErrorMessage = " El Campo {0} debe ser menor de {1} Caracteres")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Detalle")]
        public string? Description { get; set; }

        [Display(Name = "Activo")]
        public bool Active { get; set; }

        [Display(Name = "Asignada")]
        public bool Assigned { get; set; }


        //A que Corporacion Pertenece
        public int CorporateId { get; set; }

        public Corporate? Corporate { get; set; }

        //Relaciones en Doble direccion
        public ICollection<Server>? Servers { get; set; }
    }
}
