using Tic.Shared.Entites;
using System.ComponentModel.DataAnnotations;

namespace Tic.Shared.EntitiesSoft
{
    public class DocumentType
    {
        [Key]
        public int DocumentTypeId { get; set; }

        [Display(Name = "Documento")]
        [MaxLength(5, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string DocumentName { get; set; } = null!;

        [MaxLength(200, ErrorMessage = "El {0} no puede tener mas de {1} Caracteres.")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Descripcion")]
        public string? Descripcion { get; set; }

        [Display(Name = "Activo")]
        public bool Active { get; set; }


        //A que Corporacion Pertenece
        public int CorporateId { get; set; }

        public Corporate? Corporate { get; set; }

        public ICollection<Cachier>? Cachiers { get; set; }

    }
}
