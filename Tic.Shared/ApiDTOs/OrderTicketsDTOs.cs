using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tic.Shared.ApiDTOs
{
    public class OrderTicketsDTOs
    {
        public int OrderTicketId { get; set; }

        public int OrdenControl { get; set; }

        public DateTime Date { get; set; }

        public int PlanCategoryId { get; set; }

        public int ServerId { get; set; }

        public int PlanId { get; set; }

        public string? NamePlan { get; set; }

        public decimal Rate { get; set; }

        public decimal Precio { get; set; }

        public decimal Cantidad { get; set; }

        public decimal SubTotal { get; set; }

        public decimal Impuesto { get; set; }

        public decimal Total { get; set; }

        [Display(Name = "Mikrotik")]
        public bool? Mikrotik { get; set; }

        public int CorporateId { get; set; }

    }
}
