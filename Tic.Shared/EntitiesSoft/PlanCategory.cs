using Tic.Shared.Entites;
using System.ComponentModel.DataAnnotations;

namespace Tic.Shared.EntitiesSoft
{
    public class PlanCategory
    {
        [Key]
        public int PlanCategoryId { get; set; }

        [Display(Name = "Categoria")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string PlanCategoryName { get; set; } = null!;

        [Display(Name = "Activo")]
        public bool Active { get; set; }

        //Propiedad Virtual de Consulta
        [Display(Name = "Planes")]
        public int PlanesNumer => Plans == null ? 0 : Plans.Count;


        //Relaciones
        //...
        public int CorporateId { get; set; }

        public Corporate? Corporate { get; set; }

        public ICollection<Plan>? Plans { get; set; }

        public ICollection<OrderTicket>? OrderTickets { get; set; }

        public ICollection<SellOne>? SellOnes { get; set; }
    }
}
