namespace Tic.Shared.ApiDTOs
{
    public class PlanIndexDTOs
    {
        public int PlanId { get; set; }

        public string? Servidor { get; set; }

        public string? Categoria { get; set; } = null!;

        public string PlanName { get; set; } = null!;

        public string? SpeedUp { get; set; }

        public string? SpeedDown { get; set; }

        public string? TiempoTicket { get; set; }

        public string? ShareUser { get; set; }

        public string? Precio { get; set; }

        public string? Active { get; set; }

        public string? MkId { get; set; }

        public int CorporateId { get; set; }
    }
}
