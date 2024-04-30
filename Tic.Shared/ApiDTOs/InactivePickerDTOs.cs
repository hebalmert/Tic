using System.ComponentModel.DataAnnotations;

namespace Tic.Shared.ApiDTOs
{
    public class InactivePickerDTOs
    {
        public int TicketInactiveId { get; set; }

        public string? Tiempo { get; set; }
    }
}
