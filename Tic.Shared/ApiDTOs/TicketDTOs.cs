using System.Transactions;

namespace Tic.Shared.ApiDTOs
{
    public class TicketDTOs
    {
        public string? Control { get; set; }

        public DateTime fechaTicket { get; set; }

        public string? PinTicket { get; set; }

        public string? VendedorName { get; set; }
    }
}
