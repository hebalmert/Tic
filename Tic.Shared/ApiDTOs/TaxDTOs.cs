namespace Tic.Shared.ApiDTOs
{
    public class TaxDTOs
    {
        public int TaxId { get; set; }
        public string TaxName { get; set; } = null!;

        public decimal Rate { get; set; }

        public bool Active { get; set; }

        public int CorporateId { get; set; }
    }
}
