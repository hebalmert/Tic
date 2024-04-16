using Tic.Shared.Entites;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tic.Shared.EntitiesSoft
{
    public class MarkModel
    {
        [Key]
        public int MarkModelId { get; set; }

        [Display(Name = "Modelo")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string MarkModelName { get; set; } = null!;

        public int MarkId { get; set; }

        [Display(Name = "Activo")]
        public bool Active { get; set; }

        [NotMapped]
        [Display(Name = "Marca")]
        public string? MarkName { get; set; }

        //Relaciones
        //...
        public int CorporateId { get; set; }

        public Corporate? Corporate { get; set; }

        public Mark? Mark { get; set; }

        //Relaciones en Doble direccion
        public ICollection<Server>? Servers { get; set; }
    }
}
