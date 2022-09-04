namespace XPaidMerchantIntegrationExample.Models
{
    public class CallbackRequest
    {
        public string Method { get; set; }
        public CallbackData Data { get; set; }
    }

    public class CallbackData
    {
        public string ExternalId { get; set; }
        public string Hash { get; set; }
        public InvoiceStatus Status { get; set; }
        public decimal? FromAmount { get; set; }
        public decimal ToAmount { get; set; }
        public decimal? Fee { get; set; }
        public CurrencyDTO FromCurrency { get; set; }
        public CurrencyDTO ToCurrency { get; set; }
    }
}
