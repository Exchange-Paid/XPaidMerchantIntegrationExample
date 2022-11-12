using Microsoft.AspNetCore.Mvc;
using XPaidMerchantIntegrationExample.Models;

namespace XPaidMerchantIntegrationExample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CallbackController : ControllerBase
    {
        private readonly ILogger<InvoiceController> _logger;
        private readonly InvoiceService invoiceService;

        public CallbackController(ILogger<InvoiceController> logger,
            InvoiceService invoiceService)
        {
            _logger = logger;
            this.invoiceService = invoiceService;
        }

        [HttpGet()]
        public IActionResult Index()
        {
            return Ok(invoiceService.GetAll());
        }

        [HttpPost("XPaid")]
        public IActionResult XPaidCallback([FromBody] CallbackRequest body)
        {
            invoiceService.Add(body);
            return Ok();
        }
    }
}