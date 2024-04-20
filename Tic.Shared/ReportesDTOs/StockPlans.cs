using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Tic.Shared.ReportesDTOs
{
    public class StockPlans
    {
        public int CorporateId { get; set; }

        [Required(ErrorMessage = "El campo {0} es Requerido")]
        [Display(Name = "Categoria")]
        public int PlanCategoryId { get; set; }

        [Required(ErrorMessage = "El campo {0} es Requerido")]
        [Display(Name = "Planes")]
        public int PlanId { get; set; }

        public IEnumerable<SelectListItem>? CategoryList { get; set; }

        public IEnumerable<SelectListItem>? PlanList { get; set; }
    }
}
