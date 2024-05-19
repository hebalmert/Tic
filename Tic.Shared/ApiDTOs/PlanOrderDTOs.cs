using System.Runtime.CompilerServices;

namespace Tic.Shared.ApiDTOs
{
    public class PlanOrderDTOs
    {
        public int PlanId { get; set; }

        public string PlanName { get; set; } = null!;

        public decimal RateTax { get; set; }

        public decimal SubTotal { get; set; }

        public decimal Impuesto { get; set; }

        public decimal Precio { get; set; }

    }
}
