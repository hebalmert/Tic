using System.ComponentModel.DataAnnotations;

namespace Tic.Shared.ApiDTOs
{
    public class ZoneDetailsDTOs
    {
        public int ZoneId { get; set; }

        public int StateId { get; set; }

        public int CityId { get; set; }

        public string ZoneName { get; set; } = null!;

        public bool Active { get; set; }
    }
}
