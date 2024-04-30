using Tic.Shared.Enum;

namespace Tic.Shared.ApiDTOs
{
    public class PlanSaveDTOs
    {
        public int PlanId { get; set; }

        public DateTime? DateCreated { get; set; }


        public DateTime? DateEdit { get; set; }


        public int ServerId { get; set; }


        public int PlanCategoryId { get; set; }


        public string PlanName { get; set; } = null!;


        public int SpeedUp { get; set; }


        public SpeedUpType SpeedUpType { get; set; }

        public int SpeedDown { get; set; }

        public SpeedDownType SpeedDownType { get; set; }

        public int TicketTimeId { get; set; }

        public int TicketInactiveId { get; set; }

        public int TicketRefreshId { get; set; }

        public int ShareUser { get; set; }


        public bool Proxy { get; set; }


        public bool MacCookies { get; set; }

        public bool ContinueTime { get; set; }


        //Precio o Valor del Servicio sin Impuesto
        //...//...//...//...//


        public int TaxId { get; set; }


        public decimal SubTotal { get; set; }


        public decimal Impuesto { get; set; }


        public decimal Precio { get; set; }
        //...//...//...//...//...//
        //Niveles de Precio FIN


        public bool Active { get; set; }

        public string? MkId { get; set; }


        public string? MkContinuoId { get; set; }

        public int CorporateId { get; set; }
    }
}
