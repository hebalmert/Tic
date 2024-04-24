using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Tic.Shared.Enum;

namespace Tic.Shared.Entites
{
    public class User : IdentityUser
    {
        [Required]
        [Display(Name = "First Name")]
        [MaxLength(50, ErrorMessage = "El campo no puede ser mayor a {0} de largo")]
        public string FirstName { get; set; } = null!;

        [Required]
        [MaxLength(50, ErrorMessage = "El campo no puede ser mayor a {0} de largo")]
        public string LastName { get; set; } = null!;

        [MaxLength(100, ErrorMessage = "El campo no puede ser mayor a {0} de largo")]
        public string? FullName { get; set; }

        [Required(ErrorMessage = "El {0} es Obligatorio")]
        [MaxLength(50, ErrorMessage = "El {0} no puede tener mas de {1} Caracteres.")]
        [Display(Name = "Puesto Trabajo")]
        public string Job { get; set; } = null!;


        //Identificacion de Origenes y Role del Usuario
        [Display(Name = "Origen")]
        public string? UserFrom { get; set; }

        [Display(Name = "User Type")]
        public UserType UserType { get; set; }
        //Fin.........

        [Display(Name = "Imagen")]
        public string? Photo { get; set; }

        //Para el envio de la Ruta Completa de la Imagen por API
        [NotMapped]
        [Display(Name = "FullPath")]
        public string? PhotoPath { get; set; }

        //TODO:Change Addres to Image
        public string ImageFullPath => Photo == string.Empty || Photo == null
        //? $"https://localhost:7160/Images/NoImage.png"
        //: $"https://localhost:7160/Images/ImgUser/{Photo}";
        ? $"http://tic.nexxtplanet.net/Images/NoImage.png"
        : $"http://tic.nexxtplanet.net/Images/ImgUser/{Photo}";

        //...
        public int? CorporateId { get; set; }

        public Corporate? Corporate { get; set; }

        //Verificacion de usuario activo
        [Display(Name = "Activo")]
        public bool Activo { get; set; }

        [NotMapped]
        public string? Pass { get; set; }
    }
}
