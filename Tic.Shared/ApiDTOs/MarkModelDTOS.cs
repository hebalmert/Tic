using System.ComponentModel.DataAnnotations;

namespace Tic.Shared.ApiDTOs
{
    public class MarkModelDTOS
    {
        public int MarkModelId { get; set; }

        public string MarkModelName { get; set; } = null!;

    }
}
