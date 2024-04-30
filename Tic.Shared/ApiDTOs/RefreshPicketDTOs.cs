using System.ComponentModel.DataAnnotations;

namespace Tic.Shared.ApiDTOs
{
    public class RefreshPicketDTOs
    {
        public int TicketRefreshId { get; set; }

        public string? Tiempo { get; set; }
    }
}
