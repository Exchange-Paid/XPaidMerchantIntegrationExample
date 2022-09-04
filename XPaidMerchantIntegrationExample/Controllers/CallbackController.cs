using Microsoft.AspNetCore.Mvc;
using XPaidMerchantIntegrationExample.Models;

namespace XPaidMerchantIntegrationExample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CallbackController : ControllerBase
    {
        private readonly ILogger<InvoiceController> _logger;

        public CallbackController(ILogger<InvoiceController> logger)
        {
            _logger = logger;
        }

        [HttpPost("XPaid")]
        public IActionResult XPaidCallback([FromBody] CallbackRequest body)
        {

            return Ok();
        }
    }
}