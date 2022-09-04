namespace XPaidMerchantIntegrationExample.Models
{
    public class CurrencyDTO
    {
        public long Id { get; set; }
        public string Symbol { get; set; }
        public string Icon { get; set; }
        public CurrencyType CurrencyType { get; set; }
        public int Decimal { get; set; }
    }
}
