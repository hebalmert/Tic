using System.ComponentModel.DataAnnotations;
using Tic.Shared.Entites;

namespace Tic.Shared.EntitiesSoft
{
    public class SellPackDetail
    {
        public int SellPackDetailId { get; set; }

        [Required(ErrorMessage = "La {0} es Obligatorio")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe Seleccionar un {0}")]
        [Display(Name = "SellPack")]
        public int SellPackId { get; set; }

        [Required(ErrorMessage = "La {0} es Obligatorio")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe Seleccionar un {0}")]
        [Display(Name = "Ticket")]
        public int OrderTicketDetailId { get; set; }


        //...
        public int CorporateId { get; set; }

        public Corporate? Company { get; set; }

        public SellPack? SellPack { get; set; }

        public OrderTicketDetail? OrderTicketDetail { get; set; }
    }
}
