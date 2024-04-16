using Tic.Shared.Entites;
using System.ComponentModel.DataAnnotations;

namespace Tic.Shared.EntitiesSoft
{
    public class Mark
    {
        [Key]
        public int MarkId { get; set; }

        [Display(Name = "Marca")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string MarkName { get; set; } = null!;

        [Display(Name = "Activo")]
        public bool Active { get; set; }

        //Propiedad Virtual de Consulta
        [Display(Name = "Modelos")]
        public int ModelNumber => MarkModels == null ? 0 : MarkModels.Count;

        //Relaciones
        //...
        public int CorporateId { get; set; }

        public Corporate? Corporate { get; set; }

        public ICollection<MarkModel>? MarkModels { get; set; }

        //Relaciones en Doble direccion
        public ICollection<Server>? Servers { get; set; }
    }
}
