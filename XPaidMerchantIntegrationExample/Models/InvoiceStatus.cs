namespace XPaidMerchantIntegrationExample.Models
{
    public enum InvoiceStatus
    {
        PendingPayment = 1,
        Processing,
        Done,
        Canceled,
        Expired
    }
}
