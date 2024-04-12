using System.ComponentModel.DataAnnotations;

namespace Tic.Shared.Entites
{
    public class Country
    {
        public int CountryId { get; set; }

        [Required(ErrorMessage = "El Campo {0} es Obligatorio")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener mas de {1} Caracter")]
        [Display(Name = "Pais")]
        public string Name { get; set; } = null!;

        [MaxLength(10, ErrorMessage = "El campo {0} no puede tener mas de {1} Caracter")]
        [Display(Name = "Cod Phone")]
        public string? CodPhone { get; set; }

        //Propiedad Virtual de consulta
        [Display(Name = "Estado")]
        public int StatesNumber => States == null ? 0 : States.Count;

        //Relacioens en doble via
        public ICollection<State>? States { get; set; }

        public ICollection<Corporate>? Corporates { get; set; }
    }
}
