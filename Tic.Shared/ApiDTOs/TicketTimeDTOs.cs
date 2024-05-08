namespace Tic.Shared.ApiDTOs
{
    public class TicketTimeDTOs
    {
        public int TicketTimeId { get; set; }

        public string? Tiempo { get; set; }

        public int? Orden { get; set; }

        public bool Activo { get; set; }

        public string? ScriptConsumo { get; set; }

        public string? ScriptContinuo { get; set; }
    }
}
