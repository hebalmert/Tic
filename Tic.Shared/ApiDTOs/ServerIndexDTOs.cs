using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Tic.Shared.ApiDTOs
{
    public class ServerIndexDTOs
    {
        public int ServerId { get; set; }

        public string ServerName { get; set; } = null!;

        public string IpNetwork { get; set; } = null!;

        public string Usuario { get; set; } = null!;

        public string Clave { get; set; } = null!;

        public int ApiPort { get; set; }

        public string Zona { get; set; } = null!;

        public string? Activo { get; set; }

        public int CorporateId { get; set; }
    }
}
