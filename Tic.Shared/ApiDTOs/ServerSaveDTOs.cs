namespace Tic.Shared.ApiDTOs
{
    public class ServerSaveDTOs
    {
        public int ServerId { get; set; }

        public string ServerName { get; set; } = null!;

        public int IpNetworkId { get; set; }

        public string Usuario { get; set; } = null!;

        public string Clave { get; set; } = null!;

        public string ClaveConfirm { get; set; } = null!;

        public string WanName { get; set; } = null!;

        public int ApiPort { get; set; }

        public int MarkId { get; set; }

        public int MarkModelId { get; set; }

        public int ZoneId { get; set; }

        public bool Active { get; set; }
    }
}
