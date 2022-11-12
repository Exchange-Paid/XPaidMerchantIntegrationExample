using XPaidMerchantIntegrationExample.Models;

namespace XPaidMerchantIntegrationExample
{
    public class InvoiceService
    {
        private readonly List<CallbackRequest> Items = new List<CallbackRequest>();

        public void Add(CallbackRequest data)
        {
            Items.Add(data);
        }

        public CallbackRequest[] GetAll()
        {
            return Items.ToArray();
        }
    }
}
