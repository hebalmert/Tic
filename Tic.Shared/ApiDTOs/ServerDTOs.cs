namespace Tic.Shared.ApiDTOs
{
    public class ServerDTOs
    {
        public int ServerId { get; set; }

        public string ServerName { get; set; } = null!;

        public string IpNetwork { get; set; } = null!;

        public string Usuario { get; set; } = null!;

        public string Clave { get; set; } = null!;

        public int ApiPort { get; set; }

        public string WanName { get; set; } = null!;

        public string Marka { get; set; } = null!;

        public string MarkModelo { get; set; } = null!;

        public string Estado { get; set; } = null!;

        public string Ciudad { get; set; } = null!;

        public string Zona { get; set; } = null!;

        public string Activo { get; set; } = null!;

        public int CorporateId { get; set; }
    }
}
