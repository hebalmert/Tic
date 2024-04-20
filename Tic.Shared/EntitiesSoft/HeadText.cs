using System.ComponentModel.DataAnnotations;
using Tic.Shared.Entites;

namespace Tic.Shared.EntitiesSoft
{
    public class HeadText
    {
        public int HeadTextId { get; set; }

        [Required(ErrorMessage = "La {0} es Obligatorio")]
        [MaxLength(512, ErrorMessage = "El campo no puede ser mayor a {0} de largo")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Text Encabezado")]
        public string? TextoEncabezado { get; set; }

        //...
        public int CorporateId { get; set; }

        public Corporate? Corporate { get; set; }
    }
}
