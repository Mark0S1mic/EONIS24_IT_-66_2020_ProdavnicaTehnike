using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProdavnicaTehnikeBekend.Models;
using ProdavnicaTehnikeBekend.Models.DTOs;
using Stripe.Checkout;
using System.Threading.Tasks;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class CheckoutController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<CheckoutController> _logger;
        private static string s_wasmClientURL = string.Empty;

        public CheckoutController(IConfiguration configuration, ILogger<CheckoutController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult> CheckoutOrder([FromBody] ProizvodDto product, [FromServices] IServiceProvider sp)
        {
            if (product == null)
            {
                _logger.LogError("Product data is missing.");
                return BadRequest("Product data is missing.");
            }

            try
            {
                var referer = Request.Headers.Referer;
                s_wasmClientURL = referer[0];

                var server = sp.GetRequiredService<IServer>();
                var serverAddressesFeature = server.Features.Get<IServerAddressesFeature>();

                string? thisApiUrl = null;

                if (serverAddressesFeature is not null)
                {
                    thisApiUrl = serverAddressesFeature.Addresses.FirstOrDefault();
                }

                if (thisApiUrl is not null)
                {
                    var sessionId = await CheckOut(product, thisApiUrl);
                    var pubKey = _configuration["Stripe:PubKey"];

                    var checkoutOrderResponse = new CheckoutOrderResponse()
                    {
                        SessionId = sessionId,
                        PubKey = pubKey
                    };

                    return Ok(checkoutOrderResponse);
                }
                else
                {
                    _logger.LogError("API URL could not be determined.");
                    return StatusCode(500, "API URL could not be determined.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during the checkout process.");
                return StatusCode(500, "An error occurred during the checkout process.");
            }
        }

        [NonAction]
        public async Task<string> CheckOut(ProizvodDto product, string thisApiUrl)
        {
            var options = new SessionCreateOptions
            {
                SuccessUrl = $"{thisApiUrl}/checkout/success?sessionId={{CHECKOUT_SESSION_ID}}",
                CancelUrl = s_wasmClientURL + "failed",
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
                {
                    new()
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long?)(product.CenaProizvoda * 100), // Stripe expects the amount in cents
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = product.NazivProizvoda,
                                Description = product.OpisProizvoda,
                            },
                        },
                        Quantity = 1,
                    },
                },
                Mode = "payment"
            };

            var service = new SessionService();
            var session = await service.CreateAsync(options);

            return session.Id;
        }

        [HttpGet("success")]
        public ActionResult CheckoutSuccess(string sessionId)
        {
            var sessionService = new SessionService();
            var session = sessionService.Get(sessionId);

            var total = session.AmountTotal.Value;
            var customerEmail = session.CustomerDetails.Email;

            return Redirect(s_wasmClientURL + "success");
        }
    }
}
