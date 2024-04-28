using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Tic.Shared.EntitiesSoft;
using Tic.Shared.Enum;

namespace Tic.Shared.ApiDTOs
{
    public class PlanDTOs
    {
        public int PlanId { get; set; }

        public DateTime? DateCreated { get; set; }

        public DateTime? DateEdit { get; set; }

        public string? Server { get; set; }

        public string? PlanCategory { get; set; }

        public string PlanName { get; set; } = null!;

        public string? SpeedUp { get; set; }

        public string? SpeedDown { get; set; }

        public string? TicketTime { get; set; }

        public string? TicketInactive { get; set; }

        public string? TicketRefresh { get; set; }

        public string? ShareUser { get; set; }

        public bool Proxy { get; set; }

        public bool MacCookies { get; set; }

        public bool ContinueTime { get; set; }

        public string? RateTax { get; set; }

        public string? SubTotal { get; set; }

        public string? Impuesto { get; set; }

        public string? Precio { get; set; }

        public string? Active { get; set; }

        public string? MkId { get; set; }

        public string? MkContinuoId { get; set; }

        public int CorporateId { get; set; }
    }
}
