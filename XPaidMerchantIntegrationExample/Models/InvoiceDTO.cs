namespace XPaidMerchantIntegrationExample.Models
{
    public class InvoiceDTO
    {
        public string MerchantName { get; set; }
        public string ExternalId { get; set; }
        public decimal? FromAmount { get; set; }
        public decimal ToAmount { get; set; }
        public decimal? Fee { get; set; }
        public InvoiceStatus Status { get; set; }
        public InvoiceAcceptingType AcceptionType { get; set; }
        public string CallbackUrl { get; set; }
        public string SuccessUrl { get; set; }
        public string FailureUrl { get; set; }
        public string Hash { get; set; }
        public string Link { get; set; }
        public long CreatedAt { get; set; }
        public long StatusUpdatedAt { get; set; }
        public CurrencyDTO FromCurrency { get; set; }
        public CurrencyDTO ToCurrency { get; set; }
    }
}
