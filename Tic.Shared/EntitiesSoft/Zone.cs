using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Tic.Shared.Entites;

namespace Tic.Shared.EntitiesSoft
{
    public class Zone
    {
        [Key]
        public int ZoneId { get; set; }

        [Required(ErrorMessage = "El Campo {0} es Requerido")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe Seleccionar un {0}")]
        [Display(Name = "Depart/Estado")]
        public int StateId { get; set; }

        [Required(ErrorMessage = "El Campo {0} es Requerido")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe Seleccionar un {0}")]
        [Display(Name = "Ciudad")]
        public int CityId { get; set; }

        [Display(Name = "Zona")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string ZoneName { get; set; } = null!;

        [Display(Name = "Activo")]
        public bool Active { get; set; }

        //Virtuales No Mapeadas
        [NotMapped]
        [Range(1, double.MaxValue, ErrorMessage = "Debe Seleccionar un {0}")]
        public IEnumerable<SelectListItem>? ListState { get; set; }

        [NotMapped]
        [Range(1, double.MaxValue, ErrorMessage = "Debe Seleccionar un {0}")]
        public IEnumerable<SelectListItem>? ListCities { get; set; }

        [NotMapped]
        public int CountryId { get; set; }

        //Relaciones
        //...
        public int CorporateId { get; set; }

        public Corporate? Corporate { get; set; }

        public State? State { get; set; }

        public City? City { get; set; }
    }
}
