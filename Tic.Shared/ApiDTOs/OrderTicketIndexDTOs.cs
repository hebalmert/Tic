namespace Tic.Shared.ApiDTOs
{
    public class OrderTicketIndexDTOs
    {
        public int OrderTicketId { get; set; }

        public int OrdenControl { get; set; }

        public string? Date { get; set; }

        public string? NameServer { get; set; }

        public string? NamePlan { get; set; }

        public decimal Cantidad { get; set; }

        public decimal Total { get; set; }

        public string? Mikrotik { get; set; }
    }
}
