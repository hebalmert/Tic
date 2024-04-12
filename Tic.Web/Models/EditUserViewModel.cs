using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Tic.Web.Models
{
    public class EditUserViewModel
    {
        public string Id { get; set; } = null!;

        [MaxLength(20)]
        [Required]
        public string Document { get; set; } = null!;

        [Display(Name = "Nombres")]
        [MaxLength(50)]
        [Required]
        public string FirstName { get; set; } = null!;

        [Display(Name = "Apllidos")]
        [MaxLength(50)]
        [Required]
        public string LastName { get; set; } = null!;

        [Display(Name = "Nombre")]
        [MaxLength(100)]
        [Required]
        public string FullName { get; set; } = null!;

        [MaxLength(100)]
        public string Address { get; set; } = null!;

        [Display(Name = "Telefono")]
        [MaxLength(20)]
        public string PhoneNumber { get; set; } = null!;

        [Display(Name = "Imagen")]
        public string ImageId { get; set; } = null!;

        //TODO: Pending to put the correct paths
        [Display(Name = "Imagen")]
        public string ImageFullPath => ImageId == string.Empty
        //? $"https://localhost:44361/Images/noimage.png"
        //: $"https://localhost:44361/users/{ImageId}";
        ? $"http://tickets.nexxtplanet.net/Images/noimage.png"
        : $"http://tickets.nexxtplanet.net/users/{ImageId}";

        //? $"http://onven.linkonext.com/Images/noimage.png"
        //: $"http://onven.linkonext.com/users/{ImageId}";
        //? $"https://onven.azurewebsites.net/images/noimage.png"
        //: $"https://onven.blob.core.windows.net/users/{ImageId}";


        [Display(Name = "Imagen")]
        public IFormFile ImageFile { get; set; } = null!;

        [Required]
        [Display(Name = "Paises")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a country.")]
        public int CountryId { get; set; }

        public IEnumerable<SelectListItem>? ListCountries { get; set; }

        [Required]
        [Display(Name = "Estados")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a department.")]
        public int DepartmentId { get; set; }

        public IEnumerable<SelectListItem>? ListStates { get; set; }

        [Required]
        [Display(Name = "Ciudades")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a city88888.")]
        public int CityId { get; set; }

        public IEnumerable<SelectListItem>? ListCities { get; set; }
    }
}
