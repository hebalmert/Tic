﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Tic.Shared.Enum;
using Tic.Shared.EntitiesSoft;

namespace Tic.Shared.Entites
{
    public class Manager
    {
        [Key]
        public int ManagerId { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = "Debe Seleccionar un Item")]
        [Required(ErrorMessage = "El {0} es Obligatorio")]
        [Display(Name = "Corporacion")]
        public int CorporateId { get; set; }

        [Required(ErrorMessage = "El {0} es Obligatorio")]
        [MaxLength(25, ErrorMessage = "El {0} no puede tener mas de {1} Caracteres.")]
        [Display(Name = "Documento")]
        public string Document { get; set; } = null!;

        [Required(ErrorMessage = "El {0} es Obligatorio")]
        [MaxLength(50, ErrorMessage = "El {0} no puede tener mas de {1} Caracteres.")]
        [Display(Name = "Nombre")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "El {0} es Obligatorio")]
        [MaxLength(50, ErrorMessage = "El {0} no puede tener mas de {1} Caracteres.")]
        [Display(Name = "Apellido")]
        public string LastName { get; set; } = null!;

        [MaxLength(100, ErrorMessage = "El {0} no puede tener mas de {1} Caracteres.")]
        [Display(Name = "Nombre")]
        public string? FullName { get; set; }

        [Required(ErrorMessage = "El {0} es Obligatorio")]
        [MaxLength(25, ErrorMessage = "El {0} no puede tener mas de {1} Caracteres.")]
        [Display(Name = "Telefono")]
        public string PhoneNumber { get; set; } = null!;

        [Required(ErrorMessage = "El {0} es Obligatorio")]
        [MaxLength(256, ErrorMessage = "El campo no puede ser mayor a {0} de largo")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Direccion")]
        public string Address { get; set; } = null!;

        //[Remote(action: "CheckEmail", controller: "Managers")]
        [MaxLength(256, ErrorMessage = "El campo no puede ser mayor a {0} de largo")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string UserName { get; set; } = null!;

        [Required(ErrorMessage = "El {0} es Obligatorio")]
        [MaxLength(50, ErrorMessage = "El {0} no puede tener mas de {1} Caracteres.")]
        [Display(Name = "Puesto Trabajo")]
        public string Job { get; set; } = null!;

        [Display(Name = "Tipo Usuario")]
        public UserType UserType { get; set; }

        [Display(Name = "Foto")]
        public string? Photo { get; set; }

        [NotMapped]
        [Display(Name = "Imagen")]
        public IFormFile? ImageFile { get; set; }

        [Display(Name = "Activo")]
        public bool Activo { get; set; }

        //TODO: Pending to put the correct paths
        [Display(Name = "Imagen")]
        public string ImageFullPath => Photo == string.Empty || Photo == null
        //? $"https://localhost:7045/images/NoImage.png"
        //: $"https://localhost:7045/images/ImgUser/{Photo}";
        ? $"http://tic.nexxtplanet.net/images/NoImage.png"
        : $"http://tic.nexxtplanet.net/images/ImgUser/{Photo}";

        [Display(Name = "Full Name")]
        public string FullNameWithDocument => $"{FirstName} {LastName} - {Document}";        //Armado de Combos
        [NotMapped]
        [Range(1, double.MaxValue, ErrorMessage = "Debe Seleccionar un {0}")]
        public IEnumerable<SelectListItem>? ListCorporate { get; set; }

        [Display(Name = "Compañia")]
        public Corporate? Corporate { get; set; }


        public ICollection<SellOne>? SellOnes { get; set; }

        public ICollection<SellPack>? SellPacks { get; set; }
    }
}
