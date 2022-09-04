using Microsoft.AspNetCore.Mvc;

namespace XPaidMerchantIntegrationExample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InvoiceController : ControllerBase
    {
        private readonly ILogger<InvoiceController> _logger;
        private readonly XPaidClient _xPaidClient;

        public InvoiceController(ILogger<InvoiceController> logger)
        {
            _logger = logger;
            _xPaidClient = new XPaidClient();
        }

        [HttpGet("View")]
        public IActionResult View(string externalId)
        {
            return Ok(_xPaidClient.GetInvoice(externalId));
        }

        [HttpPost("Create")]
        public IActionResult Create(decimal amount, string currency)
        {
            return Ok(_xPaidClient.CreateInvoice(
                amount,
                currency,
                Guid.NewGuid().ToString(),
                "https://localhost:7182/Callback/XPaid"));
        }
    }
}